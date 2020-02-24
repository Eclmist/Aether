using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : Singleton<VisibilityManager>
{
    private const int THREADGROUP_SIZE_X = 8;
    private const int THREADGROUP_SIZE_Y = 8;
    private const int THREADGROUP_SIZE_Z = 8;

    private const int COMPUTE_KERNEL = 0;
    private const int RESET_KERNEL = 1;
    private const int DEBUG_KERNEL = 2;

    public class VisibilityModifier
    {
        public Vector3 m_Position;
        public float m_Radius = 1;
        public float m_TargetVisibility = 1;
    }

    [SerializeField]
    [Range(1, 16)]
    private int m_NumDispatchPerFrame = 4;

    [SerializeField]
    private bool m_DebugView = false;

    private const int m_TextureSizeX = 256;
    private const int m_TextureSizeY = 32;
    private const int m_TextureSizeZ = 256;

    private ComputeShader m_VisibilityCS;
    private RenderTexture m_WorldVisibilityTexture;
#if UNITY_EDITOR
    private RenderTexture m_VisibilityDebugTexture;
#endif

    private List<VisibilityModifier> m_PersistentModifierList = new List<VisibilityModifier>();
    private Queue<VisibilityModifier> m_OneShotModifierQueue = new Queue<VisibilityModifier>();

    private void Start()
    {
        LoadCS();
        InitGlobalTextures();
        SetShaderGlobals();
    }

    private void OnDestroy()
    {
        if (m_WorldVisibilityTexture != null)
        {
            Destroy(m_WorldVisibilityTexture);
            m_WorldVisibilityTexture = null;

#if UNITY_EDITOR
            Destroy(m_VisibilityDebugTexture);
            m_VisibilityDebugTexture = null;
#endif
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
        Shader.SetGlobalVector("_WorldVisibilityTextureSize", new Vector3(m_TextureSizeX, m_TextureSizeY, m_TextureSizeZ));
    }

    private void InitGlobalTextures()
    {
        if (m_WorldVisibilityTexture == null)
        {
            RenderTextureDescriptor desc = new RenderTextureDescriptor(m_TextureSizeX, m_TextureSizeY, RenderTextureFormat.R8);
            desc.volumeDepth = m_TextureSizeZ;
            desc.depthBufferBits = 0;
            desc.enableRandomWrite = true;
            desc.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            m_WorldVisibilityTexture = new RenderTexture(desc);
            m_WorldVisibilityTexture.Create();
        }

#if UNITY_EDITOR
        m_VisibilityDebugTexture = new RenderTexture(m_TextureSizeX, m_TextureSizeZ, 0, RenderTextureFormat.R8);
        m_VisibilityDebugTexture.enableRandomWrite = true;
        m_VisibilityDebugTexture.Create();
#endif
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

#if UNITY_EDITOR
        VisibilityDebugger();
#endif
    }

#if UNITY_EDITOR
    private void VisibilityDebugger()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            m_DebugView = !m_DebugView;

        if (!m_DebugView)
            return;

        m_VisibilityCS.SetTexture(DEBUG_KERNEL, "WorldVisibilityResult", m_WorldVisibilityTexture);
        m_VisibilityCS.SetTexture(DEBUG_KERNEL, "DebugTexture", m_VisibilityDebugTexture);
        m_VisibilityCS.Dispatch(DEBUG_KERNEL, m_TextureSizeX / THREADGROUP_SIZE_X, m_TextureSizeZ / THREADGROUP_SIZE_Z, 1);
    }
#endif

    private void DispatchModifier(VisibilityModifier mod, bool isInstant)
    {
        // Let the center of the texture be world origin (0,0), so every position should be
        // offset by adding half of texture width and height
        Vector3 modPosTangentSpace = mod.m_Position + new Vector3(m_TextureSizeX / 2, 0, m_TextureSizeZ / 2);

        int numThreadGroupX = (int)Mathf.Ceil((mod.m_Radius * 2) / THREADGROUP_SIZE_X);
        int numThreadGroupY = (int)Mathf.Ceil((mod.m_Radius * 2) / THREADGROUP_SIZE_Y);
        int numThreadGroupZ = (int)Mathf.Ceil((mod.m_Radius * 2) / THREADGROUP_SIZE_Z);

        m_VisibilityCS.SetVector("ModifierPosition", modPosTangentSpace);
        m_VisibilityCS.SetFloat("ModifierRadius", mod.m_Radius);
        m_VisibilityCS.SetFloat("TargetVisibility", mod.m_TargetVisibility);
        m_VisibilityCS.SetFloat("UpdateSpeed", isInstant ? 1 : Time.deltaTime * 6);
        m_VisibilityCS.SetTexture(COMPUTE_KERNEL, "WorldVisibilityResult", m_WorldVisibilityTexture);

        m_VisibilityCS.Dispatch(COMPUTE_KERNEL, numThreadGroupX, numThreadGroupY, numThreadGroupZ);
    }

#if UNITY_EDITOR
    public void OnGUI()
    {
        if (!m_DebugView)
            return;

        if (Event.current.type != EventType.Repaint)
            return;

        Graphics.DrawTexture(new Rect(0, 0, 512, 512), m_VisibilityDebugTexture);
    }
#endif

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
