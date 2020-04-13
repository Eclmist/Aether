using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class RevealableObject : MonoBehaviour
{
    private static float m_TransitionSpeed = 4.0f;

    private static Shader m_StealthShader;

    [SerializeField]
    [Range(0, 5)]
    [Tooltip("Reveal the terrain area around the base of the object so that there is little" +
        "disconnect between the two reveal systems.")]
    private float m_TerrainRevealRadius = 1.5f;

    private Material[] m_RevealMaterials;
    private Material[] m_StealthMaterials;

    private float m_Opacity = 0.01f;
    private float m_TargetOpacity = 0;
    private float m_BBHeightInv;

    private bool m_InRevealableMode = true;

    private Renderer m_Renderer;

    private void Awake()
    {
        if (m_StealthShader == null)
        {
            m_StealthShader = (Shader)Resources.Load("Stealth");
            Debug.Assert(m_StealthShader != null, "Stealth Shader was not found");
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer == null)
            Destroy(this);

        // Disable renderer until revealed
        m_Renderer.enabled = false;

        // Set bbox height
        float pivotYOff = Mathf.Abs(transform.position.y - (m_Renderer.bounds.center.y - m_Renderer.bounds.extents.y));
        float height = m_Renderer.bounds.size.y;
        m_BBHeightInv = 1.0f / height;

        int numMaterials = m_Renderer.materials.Length;
        m_RevealMaterials = new Material[numMaterials];
        m_StealthMaterials = new Material[numMaterials];
        for (int i = 0; i < numMaterials; i++)
        {
            m_Renderer.materials[i].SetFloat("_PivotYOff", pivotYOff);
            m_Renderer.materials[i].SetFloat("_Height", height);

            m_RevealMaterials[i] = m_Renderer.materials[i];
            m_StealthMaterials[i] = new Material(m_StealthShader);
            m_StealthMaterials[i].SetTexture("_MainTex", m_RevealMaterials[i].GetTexture("_MainTex"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InRevealableMode)
        {
            if (Mathf.Abs(m_Opacity - m_TargetOpacity) < 0.001f)
                return;

            if (m_Opacity < m_TargetOpacity)
            {
                m_Opacity += m_TransitionSpeed * Time.deltaTime * m_BBHeightInv;
                if (m_Opacity > m_TargetOpacity)
                    m_Opacity = m_TargetOpacity;
            }
            else if (m_Opacity > m_TargetOpacity)
            {
                m_Opacity -= m_TransitionSpeed * Time.deltaTime * m_BBHeightInv;
                if (m_Opacity < m_TargetOpacity)
                    m_Opacity = m_TargetOpacity;
            }

            foreach (Material m in m_Renderer.materials)
                m.SetFloat("_Opacity", m_Opacity);
        }
    }

    public void Reveal()
    {
        SetInRevealableMode(true);

        if (m_TargetOpacity == 1)
            return;

        m_Renderer.enabled = true;
        m_TargetOpacity = 1;

        VisibilityManager.VisibilityModifier mod = new VisibilityManager.VisibilityModifier();
        mod.m_Radius = m_TerrainRevealRadius;
        mod.m_Position = transform.position;
        mod.m_IsUnreveal = true;
        VisibilityManager.Instance.RegisterVisibilityOneShot(mod);

        PlayAudioFx();
    }

    public void Hide()
    {
        SetInRevealableMode(true);

        if (m_TargetOpacity == 0)
            return;

        m_TargetOpacity = 0;

        VisibilityManager.VisibilityModifier mod = new VisibilityManager.VisibilityModifier();
        mod.m_Radius = m_TerrainRevealRadius;
        mod.m_Position = transform.position;
        mod.m_IsUnreveal = true;
        VisibilityManager.Instance.RegisterVisibilityOneShot(mod);

        PlayAudioFx();
    }

    public void Stealth()
    {
        if (m_Opacity == 0)
            SetInRevealableMode(false);
    }

    public void UnStealth()
    {
        SetInRevealableMode(true);
    }

    private void SetInRevealableMode(bool value)
    {
        if (m_InRevealableMode == value)
            return;

        m_InRevealableMode = value;
        if (m_InRevealableMode)
        {
            m_Renderer.enabled = false;
            m_Renderer.materials = m_RevealMaterials;
        }
        else
        {
            m_RevealMaterials = m_Renderer.materials;
            m_Renderer.materials = m_StealthMaterials;
            m_Renderer.enabled = true;
        }
    }

    private static float m_TimeOfLastSFX = 0;
    private void PlayAudioFx()
    {
        if (Time.time - m_TimeOfLastSFX < 0.4f)
            return;

        m_TimeOfLastSFX = Time.time;

        // Play audio
        AudioManager.m_Instance.PlaySoundAtPosition("MAGIC_Chime", transform.position, Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_TerrainRevealRadius);
    }
}
