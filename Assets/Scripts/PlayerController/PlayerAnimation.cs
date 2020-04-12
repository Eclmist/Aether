using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    private PlayerStance m_PlayerStance;
    private PlayerCombatHandler m_PlayerCombatHandler;
    private SkillHandler m_SkillHandler;

    private Vector2 m_AxisDelta;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerStance = GetComponent<PlayerStance>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
        m_SkillHandler = GetComponent<SkillHandler>();
    }

    void Update()
    {
        m_AxisDelta.x = Mathf.Clamp(Mathf.Lerp(m_AxisDelta.x, 0, Time.deltaTime * 5), -1, 1);
        m_AxisDelta.y = Mathf.Clamp(Mathf.Lerp(m_AxisDelta.y, 0, Time.deltaTime * 5), -1, 1);

        Vector2 playerInput = m_PlayerMovement.GetInputAxis();
        m_AxisDelta.x = Mathf.Abs(m_AxisDelta.x) < Mathf.Abs(playerInput.x) ? playerInput.x : m_AxisDelta.x;
        m_AxisDelta.y = Mathf.Abs(m_AxisDelta.y) < Mathf.Abs(playerInput.y) ? playerInput.y : m_AxisDelta.y;

        m_Animator.SetFloat("Velocity-XZ-Normalized-01", m_AxisDelta.magnitude);

        // TODO: Support Velocity X, Velocity Z for unarmed rotation
        if (m_PlayerStance != null && m_PlayerStance.IsCombatStance())
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
        if (m_PlayerStance != null)
            m_Animator.SetInteger("WeaponIndex", (int)m_PlayerStance.GetStance());

        // Set Jump trigger
        bool hasJumped = m_PlayerMovement.JumpedInCurrentFrame();
        if (hasJumped)
            m_Animator.SetTrigger("Jump");

        // Set combat states
        HandleCombatAnimations();

        // Set skill states
        HandleSkillAnimations();
    }

    private void HandleCombatAnimations()
    {
        if (m_PlayerCombatHandler == null)
            return;

        if (m_PlayerCombatHandler.GetAttackedInCurrentFrame())
        {
            m_Animator.SetTrigger("Attack");
            return;
        }

        if (m_PlayerMovement.DodgedBackwardsInCurrentFrame())
        {
            m_Animator.SetTrigger("Backstep");
            return;
        }

        if (m_PlayerMovement.DodgedInCurrentFrame())
        {
            m_Animator.SetTrigger("Roll");
            return;
        }

        m_Animator.SetBool("Block", m_PlayerCombatHandler.IsBlocking());
    }

    public bool IsPlayingAttackAnimation()
    {
        // TODO: Find a more elegant way to do this (animation callbacks?)
        for (int i = 0; i < m_Animator.layerCount; ++i)
            if (m_Animator.GetCurrentAnimatorStateInfo(i).IsTag("IsAttack"))
                return true;

        return false;
    }

    private void HandleSkillAnimations()
    {
        if (m_SkillHandler == null)
            return;

        if (!m_SkillHandler.GetCastInCurrentFrame())
        {
            m_Animator.SetInteger("SkillsIndex", (int)SkillItem.SkillType.NONE);
        }
        else
        {
            int currentActiveSkill = m_SkillHandler.GetCurrentActiveSkill();
            if (currentActiveSkill != (int)SkillItem.SkillType.NONE)
                m_Animator.SetInteger("SkillsIndex", currentActiveSkill);
        }
    }

    public bool IsPlayingCastingAnimation()
    {
        for (int i = 0; i < m_Animator.layerCount; i++)
            if (m_Animator.GetCurrentAnimatorStateInfo(i).IsTag("IsCasting"))
                return true;

        return false;
    }
}
