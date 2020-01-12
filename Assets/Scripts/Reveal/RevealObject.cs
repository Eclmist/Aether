using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealObject : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private LayerMask m_LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_Radius, m_LayerMask);

        foreach (Collider c in colliders)
        {
            RevealableObject target = c.GetComponent<RevealableObject>();
            
            if (target == null)
                continue;

            target.Reveal();
        }
    }
}
