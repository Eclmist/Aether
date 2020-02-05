using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            PowerUpsManager manager = c.GetComponent<PowerUpsManager>();

            if (manager != null && !manager.HasSpeedPowerUp())
            {
                manager.GoFaster();
                Destroy(gameObject);
            }
        }
    }
}