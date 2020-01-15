using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealActor : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private LayerMask m_ObjectLayerMask = new LayerMask();

    [SerializeField]
    private LayerMask m_VertexPaintMask = new LayerMask();

    [SerializeField]
    private AnimationCurve m_VertexPaintCurve = null;

#if USE_FAKE_CAMERA_REVEAL
    [SerializeField]
    private float m_RevealSpeed;
#endif

    private const float m_SqrDistanceBetweenPaints = 0.25f;
    private Vector3 m_LastPaintedPosition;

#if USE_FAKE_CAMERA_REVEAL
    // For smoother reveal, we lerp the revealer's position. This is less expensive
    // than lerping individual vertex colors;
    private Vector3 m_LastPosition;
#endif

    private void Start()
    {
#if USE_FAKE_CAMERA_REVEAL
        m_LastPosition = transform.position;
#endif
    }

    // Update Revealable Objects (the ones that fade from bottom to top in one go)
    // FixedUpdate because physics functions
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_Radius, m_ObjectLayerMask);

        foreach (Collider c in colliders)
        {
            RevealableObject target = c.GetComponent<RevealableObject>();
            
            if (target == null)
                continue;

            target.Reveal();
        }
    }

    // Update the terrain per vertex. 
    private void Update()
    {
        if (Vector3.SqrMagnitude(transform.position - m_LastPaintedPosition) >= m_SqrDistanceBetweenPaints)
        {
            PaintVertex();
            m_LastPaintedPosition = transform.position;
        }

#if USE_FAKE_CAMERA_REVEAL
        m_LastPosition = Vector3.Lerp(m_LastPosition, transform.position, Time.deltaTime * m_RevealSpeed);

        Shader.SetGlobalVector("_PaintPosition", m_LastPosition);
#endif
    }

    private void PaintVertex()
    {
        float modulatedRadius = m_Radius / 2;
        Collider[] colliders = Physics.OverlapSphere(transform.position, modulatedRadius, m_VertexPaintMask);

        foreach (Collider c in colliders)
        {
            RevealableTerrain target = c.GetComponent<RevealableTerrain>();
            
            if (target == null)
                continue;

            target.PaintAtPosition(transform.position, modulatedRadius, m_VertexPaintCurve);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
