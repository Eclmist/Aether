using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillHandler : MonoBehaviour
{

    private const int m_SkillLimit = 3;

    private Queue<ItemSkill> m_ItemSkillSlots;
    
    void Start()
    {
        AetherInput.GetPlayerActions().UseSkill.performed += UseSkillAt;
        AetherInput.GetPlayerActions().SwitchSkills.performed += SwitchSkills;
        m_ItemSkillSlots = new Queue<ItemSkill>();
    
    }

    //key bindings
    public void UseSkillAt(InputAction.CallbackContext ctx)
    {

        if (m_ItemSkillSlots.Count == 0)
            return;

        ItemSkill currentSkill = m_ItemSkillSlots.Peek();

        currentSkill.UseSkill();
        currentSkill.DecrementUses();

        if (currentSkill.HasNoMoreUses())
            RemoveSkill();
    }

    public void AddSkill(ItemSkill itemSkill)
    {

        if (m_ItemSkillSlots.Count >= m_SkillLimit)
            return;

        m_ItemSkillSlots.Enqueue(itemSkill);
        UIManager.Instance.SaveSkill(itemSkill);
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
    
}
