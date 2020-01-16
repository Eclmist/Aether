using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerFeetIK : MonoBehaviour
{
    [SerializeField]
    private float m_DistanceToGround;

    [SerializeField]
    private LayerMask m_GroundLayer;

    [SerializeField]
    private Transform m_LeftFeetPos;

    [SerializeField]
    private Transform m_RightFeetPos;

    [SerializeField]
    private float m_Radius;

    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (m_Animator)
        {
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.5f);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.5f);

            if (Physics.CheckSphere(m_LeftFeetPos.position, m_Radius, m_GroundLayer))
            {
                Vector3 leftFeetPosition = m_LeftFeetPos.position;
                leftFeetPosition.y += m_Radius;
                m_Animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFeetPosition);
            }

            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.5f);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.5f);

            if (Physics.CheckSphere(m_RightFeetPos.position, m_Radius, m_GroundLayer))
            {
                Vector3 rightFeetPosition = m_RightFeetPos.position;
                rightFeetPosition.y += m_Radius;
                m_Animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFeetPosition);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_LeftFeetPos.position, m_Radius);
        Gizmos.DrawWireSphere(m_RightFeetPos.position, m_Radius);
    }
}
