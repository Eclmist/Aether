using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    [SerializeField]
    private int m_NoOfUses;

    private Image m_SkillIcon;
    
    public abstract void UseSkill();
    public abstract void InitializeSkill();

    public bool HasNoMoreUses() 
    {
        return m_NoOfUses == 0;
    }

    public int GetNumberOfUses()
    {
        return m_NoOfUses;
    }

    public void SetNumberOfUses(int uses) 
    {
        m_NoOfUses = uses;
    }
 
    public void DecrementUses() {
        if (m_NoOfUses > 0)
            m_NoOfUses--;
    }

    public Image GetSkillsIcon() 
    {
        return m_SkillIcon;
    }

    public void SetSkillsIcon(Image icon)
    {
        m_SkillIcon = icon;
    }
}
