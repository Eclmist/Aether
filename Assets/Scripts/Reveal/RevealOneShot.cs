using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOneShot : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private bool m_IsUnreveal = false;

    private VisibilityManager.VisibilityModifier m_VisibilityModifier;

    private void Start()
    {
        m_VisibilityModifier = new VisibilityManager.VisibilityModifier();
        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = m_Radius;
        m_VisibilityModifier.m_IsUnreveal = m_IsUnreveal;
        VisibilityManager.Instance.RegisterVisibilityOneShot(m_VisibilityModifier);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
