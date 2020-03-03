using UnityEngine;

public class PlaceholderSkill : ItemSkill
{
    public override void InitializeSkill()
    {
        UIManager.Instance.SaveSkill(this);
    }

    public override void UseSkill()
    {
        Debug.Log("penis");
        UIManager.Instance.RemoveSkill();
    }
}
