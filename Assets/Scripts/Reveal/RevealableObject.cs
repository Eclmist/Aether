using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RevealableObject : MonoBehaviour
{
    private static float m_TransitionSpeed = 0.3f;

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Reveal is triggered by the player for better performance (physics accel). " +
        "The distance set on the object revealer should be upper bounded. Use this multiplier" +
        "to reduce the distance set on the player.")]
    private float m_RevealRadiusMultiplier = 1;

    private float m_Opacity = 1;
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
        m_TargetOpacity = 1; 
    }

    public void Hide()
    {
        // TODO: Find a better solution for varying pivot points
        m_TargetOpacity = 0;
    }
}
