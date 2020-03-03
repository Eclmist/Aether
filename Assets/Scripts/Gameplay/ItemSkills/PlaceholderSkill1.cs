using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill1 : ItemSkill
{
    public override void InitializeSkill()
    {
        m_NoOfUses = 3;
        m_SkillIcon = GameObject.Find("Placeholder Item 2 Skill").GetComponent<Image>();
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Unity Chan Skill Used");
    }
}
