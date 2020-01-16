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
    [Range(0, 10)]
    private float m_WeightDampingSpeed = 0.1f;
    
    [SerializeField]
    [Range(0, 1)]
    private float m_DisabledWeight = 0.2f;

    [SerializeField]
    private Transform m_EyePosition = null;

    private Animator m_Animator;

    private float m_LookAtWeightCache = 1;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_LookAtWeightCache = m_Weight;
    }

    void OnAnimatorIK()
    {
        // Find a better way to do this
        if (m_Animator.enabled == false ||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing") ||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_DisabledWeight, Time.deltaTime * m_WeightDampingSpeed);
        }
        else
        {
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_Weight, Time.deltaTime * m_WeightDampingSpeed);
        }

        Vector3 lookat = Camera.main.transform.forward;
        m_Animator.SetLookAtWeight(m_LookAtWeightCache, m_WeightBody, m_WeightHead, m_WeightEye, m_WeightClamp);
        m_Animator.SetLookAtPosition(m_EyePosition.position + lookat * m_Distance);
    }

    private void OnDrawGizmos()
    {
        Vector3 lookat = Camera.main.transform.forward;
        Gizmos.DrawLine(m_EyePosition.position, m_EyePosition.position + lookat * m_Distance);
        Gizmos.DrawSphere(m_EyePosition.position + lookat * m_Distance, 0.5f);
    }
}
