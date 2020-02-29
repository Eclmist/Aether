using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEPRECATED: This system has been replaced by the 3D texture system.
///
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
    private void Start()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_WorldSpaceVertices = m_MeshFilter.mesh.vertices;

        // Transform vertices to world space
        for (int i = 0; i < m_WorldSpaceVertices.Length; ++i)
            m_WorldSpaceVertices[i] = transform.TransformPoint(m_WorldSpaceVertices[i]);

        m_VertexColors = new Color32[m_WorldSpaceVertices.Length];
        m_TargetVertexColors = new Color32[m_WorldSpaceVertices.Length];
        m_MeshFilter.mesh.colors32 = m_VertexColors;

        StartCoroutine(Coroutine_UpdateMeshColors());
    }

    public void PaintAtPosition(RevealActor.RevealMode revealMode, Vector3 position, float radius, AnimationCurve falloff = null)
    {
        StartCoroutine(Coroutine_PaintAtPosition(revealMode, position, radius, falloff));
    }

    IEnumerator Coroutine_PaintAtPosition(RevealActor.RevealMode revealMode, Vector3 position, float radius, AnimationCurve falloff = null)
    {

        for (int i = 0; i < m_WorldSpaceVertices.Length; ++i)
        {
            float distance = (position - m_WorldSpaceVertices[i]).magnitude;

            if (distance > radius)
                continue;

            float amount;
            float currentAmount = m_TargetVertexColors[i].r;
            float startTime = Time.time;
            if (falloff != null)
                amount = falloff.Evaluate(distance / radius) * 255;
            else
                amount = (1 - Mathf.Pow(distance / radius, 4)) * 255;

            // Ignore falloff for now
            switch (revealMode)
            {
                case RevealActor.RevealMode.REVEALMODE_SHOW:
                    if (amount > currentAmount)
                    {
                        m_TargetVertexColors[i].r = (byte)amount;
                        m_IsUpdating = true;
                    }

                    amount = 255 - amount;
                    break;
                case RevealActor.RevealMode.REVEALMODE_HIDE:
                    if (amount < currentAmount)
                    {
                        m_TargetVertexColors[i].r = (byte)amount;
                        m_IsUpdating = true;
                    }
                    break;
                default:
                    Debug.LogWarning("Should not be entering default case in RevealableTerrain PaintAtPosition");
                    break;
            }

            if (startTime - Time.time >= 0.1f)
            {
                yield return null;
            }
        }

        yield return null;
    }

    IEnumerator Coroutine_UpdateMeshColors()
    {
        while (true)
        {
            if (Time.time - m_LastPaintTime < m_MinTimeBetweenPaints)
            {
                yield return null;
                continue;
            }


            if (!m_IsUpdating)
            {
                yield return null;
                continue;
            }

            Mesh mesh = m_MeshFilter.mesh;

            bool atLeastOneVertUpdated = false;


            for (int i = 0; i < m_VertexColors.Length; ++i)
            {
                float startTime = Time.time;
                // TODO: Profile if typecasting to Color is more or less efficient
                float diff = Mathf.Abs(m_VertexColors[i].r - m_TargetVertexColors[i].r);
                diff += Mathf.Abs(m_VertexColors[i].g - m_TargetVertexColors[i].g);
                diff += Mathf.Abs(m_VertexColors[i].b - m_TargetVertexColors[i].b);

                if (diff <= 1)
                    continue;

                atLeastOneVertUpdated = true;
                m_VertexColors[i] = Color32.Lerp(m_VertexColors[i], m_TargetVertexColors[i], Time.deltaTime * 2.0f);

                if (startTime - Time.time >= 0.1f)
                {
                    yield return null;
                    continue;
                }
            }

            if (!atLeastOneVertUpdated)
            {
                m_IsUpdating = false;
                continue;
            }

            m_LastPaintTime = Time.time;
            mesh.colors32 = m_VertexColors;

            yield return null;
        }
    }

    public Vector3[] GetVertexPositions()
    {
        return m_WorldSpaceVertices;
    }

    public Color32[] GetVertexColors()
    {
        return m_VertexColors;
    }
}
