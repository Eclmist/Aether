using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerNetworkHandler))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;

    private PlayerMovement m_PlayerMovement;

    private PlayerNetworkHandler m_PlayerNetworkHandler;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
    }

    void Update()
    {
        float axisMagnitude = m_PlayerMovement.GetLastKnownInput().magnitude;
        m_Animator.SetFloat("MovementInput", axisMagnitude);

        Vector3 velocity = m_PlayerMovement.GetVelocity();
        m_Animator.SetFloat("VerticalVelocity", velocity.y);

        // remove y-velocity
        velocity.y = 0;

        if (velocity.magnitude > 0.0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity.normalized), Time.deltaTime * 10);

        bool isGrounded = m_PlayerMovement.GetIsGrounded();
        // Set Grounded boolean
        m_Animator.SetBool("Grounded", isGrounded);

        // Handle Jumping callback
        if (m_PlayerMovement.GetJumpedInCurrentFrame())
            m_Animator.SetTrigger("Jumping");

        // Set walking animation speed based on players actual velocity. This allows slightly better sync between
        // animation and gameplay.
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (axisMagnitude > 0.01f)
                m_Animator.speed = m_AnimationSpeedCurve.Evaluate(axisMagnitude); // Make running animation speed match actual movement speed
            else 
                m_Animator.speed = 1;
        }

        if (m_PlayerNetworkHandler.networkObject != null)
        {
            m_PlayerNetworkHandler.networkObject.axisMagnitude = axisMagnitude;
            m_PlayerNetworkHandler.networkObject.vertVelocity = velocity.y;
            m_PlayerNetworkHandler.networkObject.rotation = transform.rotation;
            m_PlayerNetworkHandler.networkObject.grounded = isGrounded;
        }
    }

    public void TriggerVictoryAnimation()
    {
        m_Animator.SetTrigger("Scored");
    }
}
