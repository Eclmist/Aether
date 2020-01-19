using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;

    private PlayerMovement m_PlayerMovement;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float worldVelocity = m_PlayerMovement.GetAbsInput();
        m_Animator.SetFloat("Velocity", worldVelocity);
        m_Animator.SetFloat("VerticalVelocity", m_PlayerMovement.GetVerticalVelocity());

        Vector3 movementDir = m_PlayerMovement.GetMovementDirection();
        if (movementDir.magnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDir), Time.deltaTime * 10);

        bool isGrounded = m_PlayerMovement.GetIsGrounded();

        m_Animator.SetBool("Grounded", isGrounded);

        if (m_PlayerMovement.GetJumpedInCurrentFrame())
            m_Animator.SetTrigger("Jumping");

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (worldVelocity > 0.01f)
                m_Animator.speed = m_AnimationSpeedCurve.Evaluate(worldVelocity); // Make running animation speed match actual movement speed
            else 
                m_Animator.speed = 1;
        }

    }
}
