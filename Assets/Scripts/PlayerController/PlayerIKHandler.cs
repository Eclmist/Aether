using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKHandler : MonoBehaviour
{
    [System.Serializable]
    public class LookIKWeights
    {
        [Range(0, 1)]
        public float m_Weight = 0.6f;

        [Range(0, 1)]
        public float m_WeightBody = 0.1f;

        [Range(0, 1)]
        public float m_WeightHead = 1.0f;

        [Range(0, 1)]
        public float m_WeightEye = 0.8f;

        [Range(0, 1)]
        public float m_WeightClamp = 0.6f;

        [Range(0, 20)]
        public float m_Distance = 10;

        public Transform m_EyePosition = null;
    }

    [SerializeField]
    private LookIKWeights m_LookIKWeights;

    private Animator m_Animator;

    // Look IK variables
    [Header("Look IK Settings")]
    [SerializeField]
    [Range(0, 1)]
    private float m_DisabledWeight;

    [SerializeField]
    [Range(0, 10)]
    private float m_WeightDampingSpeed;

    private float m_LookAtWeightCache;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_LookAtWeightCache = m_LookIKWeights.m_Weight;
    }

    void OnAnimatorIK()
    {
        UpdateLookIK();
    }

    void UpdateLookIK()
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
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_LookIKWeights.m_Weight, Time.deltaTime * m_WeightDampingSpeed);
        }

        Vector3 lookat = Camera.main.transform.forward;
        m_Animator.SetLookAtWeight(m_LookAtWeightCache, m_LookIKWeights.m_WeightBody,
            m_LookIKWeights.m_WeightHead, m_LookIKWeights.m_WeightEye, m_LookIKWeights.m_WeightClamp);
        m_Animator.SetLookAtPosition(m_LookIKWeights.m_EyePosition.position + lookat * m_LookIKWeights.m_Distance);
    }

    private void OnDrawGizmos()
    {
        Vector3 lookat = Camera.main.transform.forward;
        Gizmos.DrawLine(m_LookIKWeights.m_EyePosition.position,
            m_LookIKWeights.m_EyePosition.position + lookat * m_LookIKWeights.m_Distance);
        Gizmos.DrawSphere(m_LookIKWeights.m_EyePosition.position + lookat * m_LookIKWeights.m_Distance, 0.5f);
    }
}
