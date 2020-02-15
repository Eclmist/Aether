using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    private PlayerMovement m_PlayerMovement;

    private Player m_Player;

    private float m_FallenDuration = 1.0f;
    private float m_MoveDelay = 2.5f;
    private float m_AxisDelta;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Player = GetComponent<Player>();
    }

    void Update()
    {
        m_AxisDelta = Mathf.Max(0, m_AxisDelta - Time.deltaTime * 5);
        m_AxisDelta = Mathf.Max(m_AxisDelta, m_PlayerMovement.GetLastKnownInput().magnitude);
        m_Animator.SetFloat("AxisDelta", m_AxisDelta);

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
            m_Animator.SetTrigger("Jump");

        if (m_Player.networkObject != null)
        {
            m_Player.networkObject.axisMagnitude = m_AxisDelta;
            m_Player.networkObject.vertVelocity = velocity.y;
            m_Player.networkObject.rotation = transform.rotation;
            m_Player.networkObject.grounded = isGrounded;
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
        m_PlayerMovement.SetParalyze();
        m_Animator.SetTrigger("HasFallen");
        yield return new WaitForSeconds(m_FallenDuration);
        m_Animator.ResetTrigger("HasFallen");
        m_PlayerMovement.ResetParalyze();
    }

    IEnumerator SetMoves()
    {
        m_PlayerMovement.SetParalyze();
        yield return new WaitForSeconds(m_MoveDelay);
        m_PlayerMovement.ResetParalyze();
    }
}
