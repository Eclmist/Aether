using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillHandler : MonoBehaviour
{
    
    public ItemSkill[] m_SkillSlots = new ItemSkill[3];
    
    void Start()
    {
        AetherInput.GetPlayerActions().UseSkill.performed += UseSkillAt;
    }

    //key bindings
    public void UseSkillAt(InputAction.CallbackContext ctx)
    {
        Debug.Log(m_SkillSlots[0] == null ? 0 : m_SkillSlots[0].GetNumberOfUses());
        Debug.Log(m_SkillSlots[1] == null ? 0 : m_SkillSlots[1].GetNumberOfUses());
        Debug.Log(m_SkillSlots[2] == null ? 0 : m_SkillSlots[2].GetNumberOfUses());

        int index = UIManager.Instance.GetSkillsIndex();
        if (m_SkillSlots[index] == null)
            return;

        ItemSkill currentSkill = m_SkillSlots[index];

        currentSkill.UseSkill();
        currentSkill.DecrementUses();
        if (currentSkill.HasNoMoreUses())
        {
            RemoveSkill(index);
            UIManager.Instance.RemoveSkill();
        }
    }

    /*
     * Attempts to add skill to player slots,
     * returns true if successful
     */
    public bool AddSkill(ItemSkill itemSkill)
    {
        if (m_SkillSlots[0] == null)
        {
            m_SkillSlots[0] = itemSkill;
            return true;
        }

        if (m_SkillSlots[1] == null)
        {
            m_SkillSlots[1] = itemSkill;
            return true;
        }
        
        if (m_SkillSlots[2] == null)
        {
            m_SkillSlots[2] = itemSkill;
                return true;
        }

        return false;
    }

    /*
     * Attempts to remove skill from player slots,
     * returns true if successful
     */
    public bool RemoveSkill(int index)
    {
        m_SkillSlots[index] = null;
        return true;
    }
    
}
