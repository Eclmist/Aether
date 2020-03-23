using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    [SerializeField]
    protected int m_NoOfUses;

    [SerializeField]
    protected int m_MaxNoOfUses;

    [SerializeField]
    protected Image m_SkillIcon;

    [SerializeField]
    protected bool m_IsGroundedSpellCast;
    
    public abstract void UseSkill();

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
        return m_IsGroundedSpellCast;
    }

    //public void SetMaxNumberOfUses(int uses)
    //{
    //    m_MaxNoOfUses = uses;
    //}

    //public void SetNumberOfUses(int uses) 
    //{
    //    m_NoOfUses = uses;
    //}

    // Check if spell requires player to be grounded
    //public void SetIsGroundOnlySpellBool(bool isGroundOnlySpell)
    //{
    //    m_IsGroundedSpellCast = isGroundOnlySpell;
    //}
    
    //public void SetUpSkill(int maxMoves, int iconIndex, bool isGroundOnlySpell)
    //{
    //    SetIsGroundOnlySpellBool(isGroundOnlySpell);
    //    SetMaxNumberOfUses(maxMoves);
    //    SetNumberOfUses(maxMoves);
    //    SetSkillsIcon(IconsManager.Instance.GetIcon(iconIndex));
    //}
 
    public void DecrementUses() {
        if (m_NoOfUses > 0)
            m_NoOfUses--;
    }

    public Image GetSkillsIcon() 
    {
        return m_SkillIcon;
    }

    //public void SetSkillsIcon(Image icon)
    //{
    //    m_SkillIcon = icon;
    //}
}
