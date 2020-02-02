using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagGoal : MonoBehaviour
{

    void OnTriggerEnter(Collider c) 
    {
        if (c.CompareTag("Player"))
        {
            FlagManager manager = c.GetComponent<FlagManager>();

            if (manager != null && manager.CheckIfFlagInPosession())
            {
                IndicateVictory(c);
            }
        }
    }

    private void IndicateVictory(Collider c)
    {
        c.GetComponent<PlayerAnimation>().TriggerVictoryAnimation();
        c.GetComponent<PlayerMovement>().SetUnmovable(true); 
    }

}
