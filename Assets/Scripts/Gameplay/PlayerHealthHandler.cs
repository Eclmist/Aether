using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : HealthHandler
{
    // Start is called before the first frame update
    public override void Damage(float amount)
    {
        StartCoroutine(SetDamaged());
    }
}