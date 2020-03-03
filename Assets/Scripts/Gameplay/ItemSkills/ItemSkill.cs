using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSkill : MonoBehaviour
{
    public int m_NoOfUses;

    [SerializeField]
    public Image m_SkillIcon;
    
    public abstract void UseSkill();
    public abstract void InitializeSkill();
}
