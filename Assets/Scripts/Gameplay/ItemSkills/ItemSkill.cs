using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    private int m_NoOfUses;

    private int m_MaxNoOfUses;

    private Image m_SkillIcon;

    private bool m_GroundedSpellCast;
    
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

    public bool HasBeenUsedBefore()
    {
        return GetNumberOfUses() < GetMaxNumberOfUses();
    }

    public bool IsGroundOnlySpell()
    {
        return m_GroundedSpellCast;
    }

    public void SetMaxNumberOfUses(int uses)
    {
        m_MaxNoOfUses = uses;
    }

    public void SetNumberOfUses(int uses) 
    {
        m_NoOfUses = uses;
    }

    // Check if spell requires player to be grounded
    public void SetIsGroundOnlySpellBool(bool isGroundOnlySpell)
    {
        m_GroundedSpellCast = isGroundOnlySpell;
    }
    
    public void SetUpSkill(int maxMoves, int iconIndex, bool isGroundOnlySpell)
    {
        SetIsGroundOnlySpellBool(isGroundOnlySpell);
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
