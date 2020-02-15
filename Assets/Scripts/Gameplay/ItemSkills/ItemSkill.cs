using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSkill : MonoBehaviour
{
    protected int m_NoOfUses;


    public abstract void UseSkill();
}
