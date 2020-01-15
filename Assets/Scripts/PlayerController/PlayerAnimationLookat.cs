using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationLookat : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float m_Weight = 0.6f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightBody = 0.1f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightHead = 1.0f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightEye = 0.8f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightClamp = 0.6f;

    [SerializeField]
    [Range(0, 20)]
    private float m_Distance = 10;

    [SerializeField]
    private Transform m_EyePosition = null;

    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (m_Animator.enabled)
        {
            Vector3 lookat = Camera.main.transform.forward;
            m_Animator.SetLookAtWeight(m_Weight, m_WeightBody, m_WeightHead, m_WeightEye, m_WeightClamp);
            m_Animator.SetLookAtPosition(m_EyePosition.position + lookat * m_Distance);
        }
        else
        {
            m_Animator.SetLookAtWeight(0);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 lookat = Camera.main.transform.forward;
        Gizmos.DrawLine(m_EyePosition.position, m_EyePosition.position + lookat * m_Distance);
        Gizmos.DrawSphere(m_EyePosition.position + lookat * m_Distance, 0.5f);
    }
}
