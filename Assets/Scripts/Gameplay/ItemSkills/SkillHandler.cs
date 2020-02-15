using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    //Requires UI
    
    //Max 3
    private int m_MaxSkillsNum = 3;
    public List<ItemSkill> m_SkillSlots = new List<ItemSkill>();
    
    //key bindings
    public void UseSkillAt(int index)
    {
        m_SkillSlots[index].UseSkill();
    }

    /*
     * Attempts to add skill to player slots,
     * returns true if successful
     */
    public bool AddSkill(ItemSkill itemSkill)
    {
        if (m_SkillSlots.Count >= 3)
        {
            return false;
        }
        
        m_SkillSlots.Add(itemSkill);
        return true;
    }

    /*
     * Attempts to remove skill from player slots,
     * returns true if successful
     */
    public bool RemoveSkill(ItemSkill itemSkill)
    {
        return m_SkillSlots.Remove(itemSkill);
    }
    
}
