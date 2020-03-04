using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill2 : ItemSkill
{
    private const int m_ICON_INDEX = 0;

    public override void InitializeSkill()
    {
        // Please Call these two methods 
        // As the Skills Handler will reset these values back to 
        // 0 and Null Respectively. 
        SetNumberOfUses(3);
        SetSkillsIcon(IconsManager.Instance.GetIcon(m_ICON_INDEX));
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Bomb Skill Used");
    }
}
