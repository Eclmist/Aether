using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStance))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    private PlayerStance m_PlayerStance;
    private PlayerCombatHandler m_PlayerCombatHandler;
    private Player m_Player;

    private Vector2 m_AxisDelta;

    void Start()
    {
        m_Player = GetComponent<Player>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerStance = GetComponent<PlayerStance>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
    }

    void Update()
    {
        m_AxisDelta.x = Mathf.Clamp(Mathf.Lerp(m_AxisDelta.x, 0, Time.deltaTime * 5), -1, 1);
        m_AxisDelta.y = Mathf.Clamp(Mathf.Lerp(m_AxisDelta.y, 0, Time.deltaTime * 5), -1, 1);

        Vector2 playerInput = m_PlayerMovement.GetLastKnownInput();
        m_AxisDelta.x = Mathf.Abs(m_AxisDelta.x) < Mathf.Abs(playerInput.x) ? playerInput.x : m_AxisDelta.x;
        m_AxisDelta.y = Mathf.Abs(m_AxisDelta.y) < Mathf.Abs(playerInput.y) ? playerInput.y : m_AxisDelta.y;
        m_Animator.SetFloat("Velocity-XZ-Normalized-01", m_AxisDelta.magnitude);

        // TODO: Support Velocity X, Velocity Z for unarmed rotation
        if (m_PlayerStance.IsCombatStance())
        {
            m_Animator.SetFloat("Velocity-X-Normalized", m_AxisDelta.x);
            m_Animator.SetFloat("Velocity-Z-Normalized", m_AxisDelta.y);
        }

        Vector3 velocity = m_PlayerMovement.GetVelocity();
        m_Animator.SetFloat("Velocity-Y-Normalized", velocity.y);

        bool isGrounded = m_PlayerMovement.IsGrounded();

        // Set Grounded boolean
        m_Animator.SetBool("Grounded", isGrounded);

        // Set Player Weapon Stance
        m_Animator.SetInteger("WeaponIndex", (int)m_PlayerStance.GetStance());

        // Set combat states
        HandleCombatAnimations();

        if (m_Player.networkObject != null)
        {
            m_Player.networkObject.axisMagnitude = m_AxisDelta.magnitude;
            m_Player.networkObject.vertVelocity = velocity.y;
            m_Player.networkObject.rotation = transform.rotation;
            m_Player.networkObject.grounded = isGrounded;
        }
    }

    void HandleCombatAnimations()
    {
        if (m_PlayerCombatHandler == null)
            return;
        
        if (m_PlayerCombatHandler.GetAttackedInCurrentFrame())
        {
            m_Animator.SetTrigger("Attacked");
            return;
        }

        if (m_PlayerMovement.DashedInCurrentFrame())
        {
            m_Animator.SetTrigger("Dash");
            m_Animator.SetInteger("DashDirection", m_PlayerMovement.GetDashDirectionIndex());
            return;
        }
    }
}
