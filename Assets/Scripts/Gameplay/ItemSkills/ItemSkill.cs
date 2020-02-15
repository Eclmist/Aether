using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSkill : MonoBehaviour
{
    protected int m_NoOfUses;
    public Sprite m_SkillIcon;
    
    public abstract void UseSkill();
    public abstract void InitializeSkill();
}
