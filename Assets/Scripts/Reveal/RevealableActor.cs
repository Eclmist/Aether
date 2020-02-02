using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealableActor : MonoBehaviour
{
    [SerializeField]
    protected LayerMask m_TerrainLayerMask;

    [SerializeField]
    protected float m_RevealRadius = 3f;

    private bool m_IsVisible = false;

    protected void Start()
    {
        StartCoroutine("UpdateVisibilityFlags");
    }

    IEnumerator UpdateVisibilityFlags()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_RevealRadius, m_TerrainLayerMask);

            float startTime = Time.time;
            bool shouldReveal = false;

            foreach (Collider c in colliders)
            {
                RevealableTerrain target = c.GetComponent<RevealableTerrain>();
                
                if (target == null)
                    continue;

                Vector3[] positions = target.GetVertexPositions();
                Color32[] colors = target.GetVertexColors();

                if (positions == null)
                    continue;

                if (colors == null)
                    continue;

                for (int i = 0; i < positions.Length; ++i)
                {
                    if (Vector3.Distance(transform.position, positions[i]) <= m_RevealRadius)
                    {
                        if (colors[i].r > 0)
                            shouldReveal = true;
                    }

                    if (Time.time - startTime > 0.5f)
                        yield return null;
                }
            }

            m_IsVisible = shouldReveal;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_RevealRadius);
    }
}
