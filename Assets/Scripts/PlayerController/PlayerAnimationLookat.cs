using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationLookat : MonoBehaviour
{
    public enum LookAtType
    {
        LOOKAT_CAMERADIR,
        LOOKAT_MOUSEPOS
    }

    [SerializeField]
    private LookAtType m_LookAtType = LookAtType.LOOKAT_CAMERADIR;

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

        Vector3 lookat;
        switch (m_LookAtType)
        {
            case LookAtType.LOOKAT_CAMERADIR:
                lookat = m_EyePosition.position + Camera.main.transform.forward * m_Distance;
                break;
            case LookAtType.LOOKAT_MOUSEPOS:
                Vector3 mouse = Input.mousePosition;
                mouse.z = 3;
                Vector3 dir = Camera.main.ScreenToWorldPoint(mouse) - m_EyePosition.position;
                lookat = m_EyePosition.position + dir.normalized * m_Distance;
                break;
            default:
                return;
        }

        m_Animator.SetLookAtWeight(m_LookAtWeightCache, m_WeightBody, m_WeightHead, m_WeightEye, m_WeightClamp);
        m_Animator.SetLookAtPosition(lookat);
    }

    private void OnDrawGizmos()
    {
        Vector3 lookat = Camera.main.transform.forward;
        Gizmos.DrawLine(m_EyePosition.position, m_EyePosition.position + lookat * m_Distance);
        Gizmos.DrawSphere(m_EyePosition.position + lookat * m_Distance, 0.5f);
    }
}
