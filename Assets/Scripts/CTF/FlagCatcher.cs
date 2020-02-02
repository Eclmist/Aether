using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCatcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
