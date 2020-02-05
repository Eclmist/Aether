using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            PowerUpsManager manager = c.GetComponent<PowerUpsManager>();

            if (manager != null && !manager.GetDoubleJump())
            {
                manager.JumpHigher();
                Destroy(gameObject);
            }
        }
    }
}