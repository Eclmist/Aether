using UnityEngine;
using UnityEngine.UI;

// Replace this with actual skills.
public class PlaceholderSkill : ItemSkill
{
    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 3;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX, false);
    }
    public override void UseSkill()
    {
        Debug.Log("Boots Skill Used");
    }

    public override string ToString()
   {
       return "Skill" + " : " + GetNumberOfUses().ToString();
   }
}
