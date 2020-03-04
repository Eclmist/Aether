using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    private int m_NoOfUses;

    private int m_MaxNoOfUses;

    private Image m_SkillIcon;
    
    public abstract void UseSkill();
    public abstract void InitializeSkill();

    public bool HasNoMoreUses() 
    {
        return m_NoOfUses == 0;
    }

    public int GetMaxNumberOfUses()
    {
        return m_MaxNoOfUses;
    }

    public int GetNumberOfUses()
    {
        return m_NoOfUses;
    }

    public void SetMaxNumberOfUses(int uses)
    {
        m_MaxNoOfUses = uses;
    }

    public void SetNumberOfUses(int uses) 
    {
        m_NoOfUses = uses;
    }

    
    public void SetUpSkill(int maxMoves, int iconIndex)
    {
        SetMaxNumberOfUses(maxMoves);
        SetNumberOfUses(maxMoves);
        SetSkillsIcon(IconsManager.Instance.GetIcon(iconIndex));
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
