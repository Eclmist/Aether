using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombatHandler))]
[RequireComponent(typeof(PlayerAnimation))]
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
        ACTION_UNSET        = 0,
        ACTION_IDLE         = 1,
        ACTION_WALK         = 2,
        ACTION_SPRINT       = 4,
        ACTION_JUMP         = 8,
        ACTION_DODGE        = 16,
        ACTION_DODGEBACK    = 32,
        ACTION_ATTACK       = 64,
        ACTION_BLOCK        = 128,
        ACTION_SHEATHE      = 256,
        // More actions to be added here (e.g., ACTION_CAPTURE, ACTION_DEATH, etc)
        ACTION_ALL          = int.MaxValue
    }

    // Action masks
    private Action m_CanWalkMask = Action.ACTION_ALL & (~Action.ACTION_ATTACK) & (~Action.ACTION_DODGE);
    private Action m_CanSprintMask = Action.ACTION_WALK;
    private Action m_CanJumpMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_SPRINT | Action.ACTION_DODGE;
    private Action m_CanDodgeMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_SPRINT | Action.ACTION_ATTACK;
    private Action m_CanDodgebackMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_SPRINT | Action.ACTION_ATTACK;
    private Action m_CanAttackMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_SPRINT | Action.ACTION_ATTACK;
    private Action m_CanBlockMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_BLOCK;
    private Action m_CanSheatheMask = Action.ACTION_IDLE | Action.ACTION_WALK | Action.ACTION_SPRINT;

    private Stance m_Stance;
    private Action m_CurrentActions;
    private PlayerMovement m_PlayerMovement;
    private PlayerCombatHandler m_PlayerCombatHandler;
    private PlayerAnimation m_PlayerAnimation;
    private bool m_IsTogglingCombatStance;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        SetWeaponActive();
    }

    void Update()
    {
        SetCurrentActions();
    }
    
    void SetCurrentActions()
    {
        m_CurrentActions = Action.ACTION_UNSET;

        if (m_PlayerMovement.GetXZVelocity().magnitude > 0)
            m_CurrentActions |= Action.ACTION_WALK;

        // TODO: Set Sprint action

        if (m_PlayerMovement.GetVelocity().y > 0 || !m_PlayerMovement.IsGrounded())
            m_CurrentActions |= Action.ACTION_JUMP;

        if (m_PlayerMovement.IsDodging())
            m_CurrentActions |= Action.ACTION_DODGE;

        if (m_PlayerMovement.IsDodging() && m_PlayerMovement.IsDodgingBackwards())
            m_CurrentActions |= Action.ACTION_DODGEBACK;

        if (m_PlayerAnimation.IsPlayingAttackAnimation())
            m_CurrentActions |= Action.ACTION_ATTACK;

        if (m_PlayerCombatHandler.IsBlocking())
            m_CurrentActions |= Action.ACTION_BLOCK;

        if (m_IsTogglingCombatStance)
            m_CurrentActions |= Action.ACTION_SHEATHE;

        if (m_CurrentActions == Action.ACTION_UNSET)
            m_CurrentActions = Action.ACTION_IDLE;
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
            case Action.ACTION_UNSET:
                return true;
            case Action.ACTION_WALK:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanWalkMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanWalkMask);
            case Action.ACTION_SPRINT:
                return false;
            case Action.ACTION_JUMP:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanJumpMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanJumpMask);
            case Action.ACTION_DODGE:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanDodgeMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanDodgeMask);
            case Action.ACTION_DODGEBACK:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanDodgebackMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanDodgebackMask);
            case Action.ACTION_ATTACK:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanAttackMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanAttackMask);
            case Action.ACTION_BLOCK:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanBlockMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanBlockMask);
            case Action.ACTION_SHEATHE:
                return BitmaskHelper.AnyBitSet(m_CurrentActions, m_CanSheatheMask) && BitmaskHelper.NoBitSet(m_CurrentActions, ~m_CanSheatheMask);
            default:
                return false;
        }
    }
}

