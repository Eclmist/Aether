using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RevealableObject : MonoBehaviour
{
    private static float m_TransitionSpeed = 0.3f;

    [SerializeField]
    [Range(0, 5)]
    [Tooltip("Reveal the terrain area around the base of the object so that there is little" +
        "disconnect between the two reveal systems.")]
    private float m_TerrainRevealRadius = 1.5f;

    private float m_Opacity = 0.01f;
    private float m_TargetOpacity = 0;
    private float m_BBHeightInv;

    private Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer == null)
            Destroy(this);

        // Disable renderer until revealed
        m_Renderer.enabled = false;

        // Set bbox height
        float pivotYOff = Mathf.Abs(transform.position.y - (m_Renderer.bounds.center.y - m_Renderer.bounds.extents.y));
        float height = m_Renderer.bounds.size.y;
        float m_BBHeightInv = 1.0f / height;

        foreach(Material m in m_Renderer.materials)
        {
            m.SetFloat("_PivotYOff", pivotYOff);
            m.SetFloat("_Height", height);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(m_Opacity - m_TargetOpacity) < 0.001f)
            return;

        if (m_Opacity < m_TargetOpacity)
        {
            m_Opacity += m_TransitionSpeed * Time.deltaTime * m_BBHeightInv;
            if (m_Opacity > m_TargetOpacity)
                m_Opacity = m_TargetOpacity;
        }
        else
        {
            m_Opacity -= m_TransitionSpeed * Time.deltaTime * m_BBHeightInv);
            if (m_Opacity < m_TargetOpacity)
                m_Opacity = m_TargetOpacity;
        }

        foreach (Material m in m_Renderer.materials)
            m.SetFloat("_Opacity", m_Opacity);
    }

    public void Reveal()
    {
        if (m_TargetOpacity == 1)
            return;

        m_Renderer.enabled = true;
        m_TargetOpacity = 1; 

        // TODO: Fix this hardcode
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_TerrainRevealRadius, LayerMask.GetMask("Terrain"));

        foreach (Collider c in colliders)
        {
            RevealableTerrain target = c.GetComponent<RevealableTerrain>();
            
            if (target == null)
                continue;

            target.PaintAtPosition(transform.position, m_TerrainRevealRadius);
        }

        PlayAudioFx();
    }

    public void Hide()
    {
        if (m_TargetOpacity == 0)
            return;

        m_TargetOpacity = 0;

        PlayAudioFx();
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
