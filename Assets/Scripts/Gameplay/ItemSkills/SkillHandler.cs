using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillHandler : MonoBehaviour
{

    private const int m_SkillLimit = 3;

    private Queue<ItemSkill> m_ItemSkillSlots = new Queue<ItemSkill>();
    
    void Start()
    {
        AetherInput.GetPlayerActions().UseSkill.performed += UseSkillAt;
    }

    private void DebugQueue()
    {
        string debugString = "";
        foreach (ItemSkill itemSkill in m_ItemSkillSlots)
        {
            debugString += itemSkill.ToString() + "\n";
        }

        Debug.Log(debugString);
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

        DebugQueue();
    }

    public void AddSkill(ItemSkill itemSkill)
    {

        if (m_ItemSkillSlots.Count >= m_SkillLimit)
            return;

        m_ItemSkillSlots.Enqueue(itemSkill);
    }

    public void RemoveSkill()
    {
        UIManager.Instance.RemoveSkill();
        m_ItemSkillSlots.Dequeue();
    }

    public void SwitchSkills()
    {
        m_ItemSkillSlots.Enqueue(m_ItemSkillSlots.Dequeue());
    }
    
}
