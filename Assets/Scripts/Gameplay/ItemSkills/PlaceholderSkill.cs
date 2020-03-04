using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill : ItemSkill
{
    public override void InitializeSkill()
    {
        SetNumberOfUses(4);
        m_SkillIcon = GameObject.Find("Placeholder Item Skill").GetComponent<Image>();
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Boots Skill Used");
    }
}
