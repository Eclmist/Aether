using UnityEngine;

public class StealthActor : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private float m_UnrenderRadius = 10;

    [SerializeField]
    private LayerMask m_ObjectLayerMask = new LayerMask();

    private void Start()
    {
        Shader.SetGlobalFloat("_MaxVisibleDistance", m_Radius);
    }

    // Update the terrain per vertex. 
    private void Update()
    {
        // Update Revealable Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_UnrenderRadius, m_ObjectLayerMask);
        float sqrRadius = m_Radius * m_Radius;

        foreach (Collider c in colliders)
        {
            RevealableObject target = c.GetComponent<RevealableObject>();

            if (target == null)
                continue;

            float sqrDist = Vector3.SqrMagnitude(transform.position - c.transform.position);
            if (sqrDist > sqrRadius)
                target.UnStealth();
            else
                target.Stealth();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
