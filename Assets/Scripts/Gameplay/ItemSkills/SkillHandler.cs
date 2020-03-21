using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SkillHandler : MonoBehaviour
{
    private PlayerStance m_PlayerStance;
    private const int m_SkillLimit = 3;
    private Queue<ItemSkill> m_ItemSkillSlots;
    private int m_CurrentActiveSkill;
    private bool m_CastInCurrentFrame;

    void Start()
    {
        m_PlayerStance = GetComponentInParent<PlayerStance>();
        AetherInput.GetPlayerActions().UseSkill.performed += UseSkillAt;
        AetherInput.GetPlayerActions().SwitchSkills.performed += SwitchSkills;
        m_ItemSkillSlots = new Queue<ItemSkill>();
    }

    private void LateUpdate()
    {
        // Reset current active skill
        
        m_CastInCurrentFrame = false;
        m_CurrentActiveSkill = 0;
    }

    //key bindings
    public void UseSkillAt(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (m_ItemSkillSlots.Count == 0)
            return;

        ItemSkill currentSkill = m_ItemSkillSlots.Peek() as ItemSkill;

        // Ground skills can only be activated if player is grounded
        if (currentSkill.IsGroundOnlySpell())
        {
            if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_CASTSPELL))
                return;
        }
        
        currentSkill.UseSkill();

        // Set current active skill
        m_CurrentActiveSkill = currentSkill.GetSkillIdentity();
        m_CastInCurrentFrame = true;
        currentSkill.DecrementUses();

        if (currentSkill.HasNoMoreUses())
            RemoveSkill();
    }

    public void AddSkill(ItemSkill itemSkill)
    {
        Debug.Log("Skill added");
        if (m_ItemSkillSlots.Count >= m_SkillLimit)
            return;

        m_ItemSkillSlots.Enqueue(itemSkill);
        UIManager.Instance.SaveSkill(itemSkill);

        Debug.Log(m_ItemSkillSlots.Peek());
    }

    public void RemoveSkill()
    {
        UIManager.Instance.RemoveSkill();
        m_ItemSkillSlots.Dequeue();
    }

    public void SwitchSkills(InputAction.CallbackContext ctx)
    {
        UIManager.Instance.SwitchPlayerSkills();
        m_ItemSkillSlots.Enqueue(m_ItemSkillSlots.Dequeue());
    }
    
    public bool GetCastInCurrentFrame()
    {
        return m_CastInCurrentFrame;
    }

    public int GetCurrentItemSkill()
    {
        return m_CurrentActiveSkill;
    }
}
