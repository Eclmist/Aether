using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    private int m_NoOfUses;

    [SerializeField]
    public Image m_SkillIcon;
    
    public abstract void UseSkill();
    public abstract void InitializeSkill();

    public bool HasNoMoreUses() {
        return m_NoOfUses == 0;
    }

    public void SetNumberOfUses(int uses) {
        m_NoOfUses = uses;
    }
 
    public void DecrementUses() {
        if (m_NoOfUses > 0)
            m_NoOfUses--;
    }
}
