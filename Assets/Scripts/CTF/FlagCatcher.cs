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
                Debug.Log("You Win!");
            }
        }
    }

}
