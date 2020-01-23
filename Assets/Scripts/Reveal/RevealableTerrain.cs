using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables per vertex painting of visibility. This is an expensive operation that should
/// only be used on terrains or large objects when RevealableObject is insufficient to do
/// the trick.
/// 
/// Use alpha channel to pack reveal data. This will give us enough bits to encode multiple
/// team's visibility.
/// 
/// </summary>
[RequireComponent(typeof(Renderer))]
public class RevealableTerrain : MonoBehaviour
{
    private MeshFilter m_MeshFilter;
    private Vector3[] m_WorldSpaceVertices;
    private Color32[] m_VertexColors;
    private Color32[] m_TargetVertexColors;

    private float m_LastPaintTime;
    private float m_MinTimeBetweenPaints = 0.0f;

    private bool m_IsUpdating;

    // Start is called before the first frame update
    void Start()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_WorldSpaceVertices = m_MeshFilter.mesh.vertices;

        // Transform vertices to world space
        for (int i = 0; i < m_WorldSpaceVertices.Length; ++i)
            m_WorldSpaceVertices[i] = transform.TransformPoint(m_WorldSpaceVertices[i]);

        m_VertexColors = new Color32[m_WorldSpaceVertices.Length];
        m_TargetVertexColors = new Color32[m_WorldSpaceVertices.Length];
        m_MeshFilter.mesh.colors32 = m_VertexColors;
    }

    private void Update()
    {
        if (Time.time - m_LastPaintTime < m_MinTimeBetweenPaints)
            return;

        if (!m_IsUpdating)
            return;

        Mesh mesh = m_MeshFilter.mesh;

        bool atLeastOneVertUpdated = false;

        for (int i = 0; i < m_VertexColors.Length; ++i)
        {
            // TODO: Profile if typecasting to Color is more or less efficient
            float diff = Mathf.Abs(m_VertexColors[i].r - m_TargetVertexColors[i].r);
            diff += Mathf.Abs(m_VertexColors[i].g - m_TargetVertexColors[i].g);
            diff += Mathf.Abs(m_VertexColors[i].b - m_TargetVertexColors[i].b);

            if (diff <= 1)
                continue;

            atLeastOneVertUpdated = true;
            m_VertexColors[i] = Color32.Lerp(m_VertexColors[i], m_TargetVertexColors[i], Time.deltaTime * 2.0f);
        }

        if (!atLeastOneVertUpdated)
        {
            m_IsUpdating = false;
            return;
        }

        m_LastPaintTime = Time.time;
        mesh.colors32 = m_VertexColors;
    }

    public void PaintAtPosition(Vector3 position, float radius, AnimationCurve falloff = null)
    {
        for (int i = 0; i < m_WorldSpaceVertices.Length; ++i)
        {
            float distance = (position - m_WorldSpaceVertices[i]).magnitude;

            if (distance > radius)
                continue;

            float amount;
            float currentAmount = m_TargetVertexColors[i].r;
            if (falloff != null)
                amount = falloff.Evaluate(distance / radius) * 255;
            else
                amount = (1 - distance / radius) * 255;

            // Ignore falloff for now
            if (amount > currentAmount)
            {
                m_TargetVertexColors[i].r = (byte)amount;
                m_IsUpdating = true;
            }
        }
    }
}
