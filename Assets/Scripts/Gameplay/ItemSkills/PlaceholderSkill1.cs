using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill1 : ItemSkill
{
    private const int m_ICON_INDEX = 0;
    public override void InitializeSkill()
    {
        SetSkillsIcon(IconsManager.Instance.GetIcon(m_ICON_INDEX));
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Unity Chan Skill Used");
    }
}
