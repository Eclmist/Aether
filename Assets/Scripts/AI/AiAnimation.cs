using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;
    
    private PlayerNetworkHandler m_PlayerNetworkHandler;
    private AICharacterControl m_AiCharacterControl;

    private float m_FallenDuration = 1.0f;

    private float m_MoveDelay = 2.5f;
    private Vector3 m_lastPos;

    void Start()
    {
        m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
        m_AiCharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        float axisMagnitude = transform.position.x - m_lastPos.x;
        m_Animator.SetFloat("MovementInput", axisMagnitude);

        Vector3 velocity = transform.position - m_lastPos;
        m_Animator.SetFloat("VerticalVelocity", velocity.y);

        // remove y-velocity
        velocity.y = 0;

        if (velocity.magnitude > 0.0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity.normalized), Time.deltaTime * 10);

        bool isGrounded = m_AiCharacterControl.GetIsGrounded();
        // Set Grounded boolean
        m_Animator.SetBool("Grounded", isGrounded);

        // Handle Jumping callback
        if (m_AiCharacterControl.GetJumpedInCurrentFrame())
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

    public void MakePlayerFall()
    {
        StartCoroutine("FallAction");
    }

    IEnumerator FallAction()
    {
        yield return StartCoroutine(SetFalls());
        yield return StartCoroutine(SetMoves());
    }

    IEnumerator SetFalls()
    {
        m_AiCharacterControl.SetParalyze();
        m_Animator.SetTrigger("HasFallen");
        yield return new WaitForSeconds(m_FallenDuration);
        m_Animator.ResetTrigger("HasFallen");
        m_AiCharacterControl.ResetParalyze();
    }

    IEnumerator SetMoves()
    {
        m_AiCharacterControl.SetParalyze();
        yield return new WaitForSeconds(m_MoveDelay);
        m_AiCharacterControl.ResetParalyze();
    }
}
