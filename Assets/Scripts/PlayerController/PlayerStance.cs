using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombatHandler))]
public class PlayerStance : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Weapon;

    public enum Stance
    {
        STANCE_UNARMED   = 0,
        STANCE_TWOHANDED = 1,
        STANCE_ONEHANDED = 2,
    }

    /**
     * Actions can be used as a bitmask to control when a seperate action can
     * be performed. For example, if the player should not be able to jump in
     * the middle of an attack, a canJump bitmask should be specified such that
     * the attack bit is set to 0.
     *
     * TODO: Support actions such as dash-jump, dash-attack where one action is
     * only possible at the start or end of of another action.
     * 
     * TODO: Support sub action types (walk forward, walk backward, attack-3, etc
     * 
     **/
    [System.Flags]
    public enum Action
    {
        ACTION_NONE     = 0,
        ACTION_WALK     = 1,
        ACTION_SPRINT   = 2,
        ACTION_JUMP     = 4,
        ACTION_DASH     = 8,
        ACTION_ATTACK   = 16,
        ACTION_BLOCK    = 32,
        ACTION_SHEATHE  = 64,
        // More actions to be added here (e.g., ACTION_CAPTURE, ACTION_DEATH, etc)
        ACTION_ALL      = int.MaxValue
    }

    // Action masks
    private Action m_CanWalkMask = Action.ACTION_ALL;
    private Action m_CanSprintMask = Action.ACTION_WALK;
    private Action m_CanJumpMask = Action.ACTION_NONE | Action.ACTION_WALK | Action.ACTION_SPRINT;
    private Action m_CanDashMask = Action.ACTION_WALK | Action.ACTION_SPRINT;
    private Action m_CanAttackMask = Action.ACTION_NONE | Action.ACTION_WALK | Action.ACTION_SPRINT;
    private Action m_CanBlockMask = Action.ACTION_NONE | Action.ACTION_WALK;
    private Action m_CanSheatheMask = Action.ACTION_NONE | Action.ACTION_WALK | Action.ACTION_SPRINT;

    private Stance m_Stance;
    private Action m_CurrentActions;
    private PlayerMovement m_PlayerMovement;
    private PlayerCombatHandler m_PlayerCombatHandler;
    private bool m_IsTogglingCombatStance;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
        SetWeaponActive();
    }

    void Update()
    {
        SetPlayerRotation();
        SetCurrentActions();
    }

    void SetPlayerRotation()
    {
        if (m_PlayerMovement.IsDashing())
            return;

        if (m_Stance == Stance.STANCE_UNARMED)
        {
            Vector3 velocity = m_PlayerMovement.GetVelocity();
            velocity.y = 0;
            if (velocity.magnitude > 0.0f)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity.normalized), Time.deltaTime * 10);
        }
        else
        {
            // Look towards camera lookat
            Vector3 cameraLookAt = Camera.main.transform.forward;
            cameraLookAt.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraLookAt.normalized), Time.deltaTime * 10);
        }
    }
    
    void SetCurrentActions()
    {
        m_CurrentActions = Action.ACTION_NONE;

        if (m_PlayerMovement.GetXZVelocity().magnitude > 0)
            m_CurrentActions |= Action.ACTION_WALK;

        // TODO: Set Sprint action

        if (m_PlayerMovement.GetVelocity().y > 0 && !m_PlayerMovement.IsGrounded())
            m_CurrentActions |= Action.ACTION_JUMP;

        if (m_PlayerMovement.IsDashing())
            m_CurrentActions |= Action.ACTION_DASH;

        if (m_PlayerCombatHandler.GetAttackedInCurrentFrame())
            m_CurrentActions |= Action.ACTION_ATTACK;

        if (m_PlayerCombatHandler.GetBlockedInCurrentFrame())
            m_CurrentActions |= Action.ACTION_BLOCK;

        if (m_PlayerCombatHandler.GetBlockedInCurrentFrame())
            m_CurrentActions |= Action.ACTION_BLOCK;

        if (m_IsTogglingCombatStance)
            m_CurrentActions |= Action.ACTION_SHEATHE;
    }

    public void SetStance(Stance targetStance)
    {
        if (m_Stance == targetStance)
            return;

        if ((m_Stance == Stance.STANCE_UNARMED || 
            targetStance == Stance.STANCE_UNARMED) &&
            !m_IsTogglingCombatStance)
            StartCoroutine(ToggleCombatStanceFlag());

        m_Stance = targetStance;
    }

    public Stance GetStance()
    {
        return m_Stance;    
    }

    public bool IsCombatStance()
    {
        return m_Stance != Stance.STANCE_UNARMED;
    }

    public void SetWeaponActive()
    {
        m_Weapon.SetActive(IsCombatStance());
    }

    IEnumerator ToggleCombatStanceFlag()
    {
        Debug.Assert(!m_IsTogglingCombatStance);
        m_IsTogglingCombatStance = true;
        yield return new WaitForSeconds(1);
        m_IsTogglingCombatStance = false;
    }

    public bool CanPerformAction(Action targetAction)
    {
        switch (targetAction)
        {
            case Action.ACTION_NONE:
                return true;
            case Action.ACTION_WALK:
                return m_CurrentActions.HasFlag(m_CanWalkMask) && !m_CurrentActions.HasFlag(~m_CanWalkMask);
            case Action.ACTION_SPRINT:
                return false;
            case Action.ACTION_JUMP:
                return m_CurrentActions.HasFlag(m_CanJumpMask) && !m_CurrentActions.HasFlag(~m_CanJumpMask);
            case Action.ACTION_DASH:
                return m_CurrentActions.HasFlag(m_CanDashMask) && !m_CurrentActions.HasFlag(~m_CanDashMask);
            case Action.ACTION_ATTACK:
                return m_CurrentActions.HasFlag(m_CanAttackMask) && !m_CurrentActions.HasFlag(~m_CanAttackMask);
            case Action.ACTION_BLOCK:
                return m_CurrentActions.HasFlag(m_CanBlockMask) && !m_CurrentActions.HasFlag(~m_CanBlockMask);
            case Action.ACTION_SHEATHE:
                return m_CurrentActions.HasFlag(m_CanSheatheMask) && !m_CurrentActions.HasFlag(~m_CanSheatheMask);
            default:
                return false;
        }

    }
}

