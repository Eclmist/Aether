using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisibilityManager : Singleton<VisibilityManager>
{
    private const int THREADGROUP_SIZE_X = 8;
    private const int THREADGROUP_SIZE_Y = 8;

    private const int COMPUTE_KERNEL = 0;
    private const int UPDATE_KERNEL = 1;
    private const int RESET_KERNEL = 2;

    public class VisibilityModifier
    {
        public Vector3 m_Position;
        public float m_Radius = 1;
        public bool m_IsUnreveal = false;
    }

    [SerializeField]
    [Range(1, 16)]
    private int m_NumDispatchPerFrame = 4;

    [SerializeField]
    private bool m_DebugView = false;

    private const int m_TextureSizeX = 512;
    private const int m_TextureSizeY = 512;

    private ComputeShader m_VisibilityCS;
    private RenderTexture m_WorldVisibilityTexture;

    private List<VisibilityModifier> m_PersistentModifierList = new List<VisibilityModifier>();
    private Queue<VisibilityModifier> m_OneShotModifierQueue = new Queue<VisibilityModifier>();

    private void Start()
    {
        LoadCS();
        InitGlobalTextures();
        SetShaderGlobals();

        SceneManager.sceneLoaded += (Scene scn, LoadSceneMode mode) =>
        {
            if (m_VisibilityCS == null)
                return;

            if (m_WorldVisibilityTexture == null)
                return;

            int numThreadGroupX = m_TextureSizeX / THREADGROUP_SIZE_X;
            int numThreadGroupY = m_TextureSizeY / THREADGROUP_SIZE_Y;
            m_VisibilityCS.SetTexture(RESET_KERNEL, "WorldVisibilityResult", m_WorldVisibilityTexture);
            m_VisibilityCS.Dispatch(RESET_KERNEL, numThreadGroupX, numThreadGroupY, 1);

            m_PersistentModifierList.Clear();
            m_OneShotModifierQueue.Clear();
        };
    }

    private void OnDestroy()
    {
        if (m_WorldVisibilityTexture != null)
        {
            Destroy(m_WorldVisibilityTexture);
            m_WorldVisibilityTexture = null;
        }
    }

    private void LoadCS()
    {
        m_VisibilityCS = (ComputeShader)Resources.Load("VisibilityCompute");

        if (m_VisibilityCS == null)
        {
            Debug.LogError("[Aether Visibility] Unable to locate visibility compute shader");
            Destroy(this);
        }
    }

    private void SetShaderGlobals()
    {
        Shader.SetGlobalTexture("_WorldVisibilityTexture", m_WorldVisibilityTexture);
        Shader.SetGlobalVector("_WorldVisibilityTextureSize", new Vector2(m_TextureSizeX, m_TextureSizeY));
    }

    private void InitGlobalTextures()
    {
        if (m_WorldVisibilityTexture == null)
        {
            RenderTextureDescriptor desc = new RenderTextureDescriptor(m_TextureSizeX, m_TextureSizeY, RenderTextureFormat.ARGB32);
            desc.depthBufferBits = 0;
            desc.enableRandomWrite = true;
            m_WorldVisibilityTexture = new RenderTexture(desc);
            m_WorldVisibilityTexture.Create();
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_NumDispatchPerFrame; ++i)
        {
            if (m_OneShotModifierQueue.Count <= 0)
                break;

            VisibilityModifier mod = m_OneShotModifierQueue.Dequeue();
            DispatchModifier(mod, true);
        }

        foreach (VisibilityModifier mod in m_PersistentModifierList)
        {
            DispatchModifier(mod, false);
        }

        VisibilityUpdate();

        VisibilityDebugger();
    }

    private void VisibilityUpdate()
    {
        int numThreadGroupX = m_TextureSizeX / THREADGROUP_SIZE_X;
        int numThreadGroupY = m_TextureSizeY / THREADGROUP_SIZE_Y;
        m_VisibilityCS.SetFloat("UpdateSpeed", Time.deltaTime * 0.75f);
        m_VisibilityCS.SetTexture(UPDATE_KERNEL, "WorldVisibilityResult", m_WorldVisibilityTexture);
        m_VisibilityCS.Dispatch(UPDATE_KERNEL, numThreadGroupX, numThreadGroupY, 1);
    }

    private void VisibilityDebugger()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            m_DebugView = !m_DebugView;

        if (!m_DebugView)
            return;
    }

    private void DispatchModifier(VisibilityModifier mod, bool isInstant)
    {
        // Let the center of the texture be world origin (0,0), so every position should be
        // offset by adding half of texture width and height
        Vector2 modPosTangentSpace = new Vector2(mod.m_Position.x, mod.m_Position.z) + 
            new Vector2(m_TextureSizeX / 2, m_TextureSizeY / 2);

        int numThreadGroupX = Mathf.Max(1, (int)Mathf.Ceil((mod.m_Radius * 2) / THREADGROUP_SIZE_X));
        int numThreadGroupY = Mathf.Max(1, (int)Mathf.Ceil((mod.m_Radius * 2) / THREADGROUP_SIZE_Y));

        m_VisibilityCS.SetVector("ModifierPosition", modPosTangentSpace);
        m_VisibilityCS.SetFloat("ModifierRadius", mod.m_Radius);
        m_VisibilityCS.SetFloat("UpdateSpeed", isInstant ? 1 : Time.deltaTime * 6);
        m_VisibilityCS.SetBool("IsUnreveal", mod.m_IsUnreveal);
        m_VisibilityCS.SetTexture(COMPUTE_KERNEL, "WorldVisibilityResult", m_WorldVisibilityTexture);

        m_VisibilityCS.Dispatch(COMPUTE_KERNEL, numThreadGroupX, numThreadGroupY, 1);
    }

    public void OnGUI()
    {
        if (!m_DebugView)
            return;

        if (Event.current.type != EventType.Repaint)
            return;

        GUI.DrawTexture(new Rect(0, 0, 256, 256), m_WorldVisibilityTexture, ScaleMode.ScaleToFit, false);
    }

    public void RegisterVisibilityOneShot(VisibilityModifier modifier)
    {
        m_OneShotModifierQueue.Enqueue(modifier);
    }

    public void RegisterPersistentVisibility(VisibilityModifier modifier)
    {
        m_PersistentModifierList.Add(modifier);

        if (m_PersistentModifierList.Count >= 4)
        {
            Debug.LogWarning("[Aether Visibility] More persistent visibility modifiers are present than expected!");
        }
    }
}
