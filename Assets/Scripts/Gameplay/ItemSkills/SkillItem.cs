using UnityEngine;
using UnityEngine.UI;

/**
 * An abstract class for all Skill Item
 * pick ups. 
 */
public abstract class SkillItem : MonoBehaviour
{
    [SerializeField]
    protected int m_NoOfUses;

    [SerializeField]
    protected int m_MaxNoOfUses;

    [SerializeField]
    protected Image m_SkillIcon;

    [SerializeField]
    protected bool m_IsGroundedSpellCast;


    public abstract void UseSkill(Transform playerTransform);

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
 
    public void DecrementUses() {
        if (m_NoOfUses > 0)
            m_NoOfUses--;
    }

    public Image GetSkillsIcon() 
    {
        return m_SkillIcon;
    }
}
