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

    private Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer == null)
            Destroy(this);

        // Set bbox height
        float pivotYOff = Mathf.Abs(transform.position.y - (m_Renderer.bounds.center.y - m_Renderer.bounds.extents.y));

        foreach(Material m in m_Renderer.materials)
        {
            m.SetFloat("_PivotYOff", pivotYOff);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(m_Opacity - m_TargetOpacity) < 0.001f)
            return;

        if (m_Opacity < m_TargetOpacity)
        {
            m_Opacity += m_TransitionSpeed * Time.deltaTime;
            if (m_Opacity > m_TargetOpacity)
                m_Opacity = m_TargetOpacity;
        }
        else
        {
            m_Opacity -= m_TransitionSpeed * Time.deltaTime;
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

        m_TargetOpacity = 1; 

        // TODO: Optimize this
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_TerrainRevealRadius);

        foreach (Collider c in colliders)
        {
            RevealableTerrain target = c.GetComponent<RevealableTerrain>();
            
            if (target == null)
                continue;

            target.PaintAtPosition(transform.position, m_TerrainRevealRadius);
        }
    }

    public void Hide()
    {
        if (m_TargetOpacity == 0)
            return;

        m_TargetOpacity = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_TerrainRevealRadius);
    }
}
