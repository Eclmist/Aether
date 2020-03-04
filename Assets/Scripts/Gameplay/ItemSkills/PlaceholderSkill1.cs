using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill1 : ItemSkill
{
    private const int m_ICON_INDEX = 0;

    private const int m_MAX_MOVES = 3;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX);
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("Unity Chan Skill Used");
    }
}
