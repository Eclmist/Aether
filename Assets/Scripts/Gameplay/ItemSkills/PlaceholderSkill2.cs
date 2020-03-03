using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill2 : ItemSkill
{
    public override void InitializeSkill()
    {
        m_NoOfUses = 2;
        m_SkillIcon = GameObject.Find("Placeholder Item 3 Skill").GetComponent<Image>();
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Bomb Skill Used");
    }
}
