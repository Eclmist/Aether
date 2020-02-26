using UnityEngine;

public class ProjectileSkill : ItemSkill
{
    public override  void InitializeSkill()
    {
        GameObject hitPoint = transform.GetChild(0).gameObject;
        if (hitPoint == null)
        {
            Debug.LogError(("Needs HitPoint under Skills for Roby's Icycles to work"));
        }
        
        hitPoint.SetActive(true);
    }

    public override void UseSkill()
    {
       
    }
}
