using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            PowerupActor interactor = c.GetComponent<PowerupActor>();
            BuffInteractor(interactor);
        }
    }

    public override void BuffInteractor(PowerupActor interactor)
    {
        if (interactor != null && !interactor.GetDoubleSpeed())
        {
            interactor.GoFaster();
            PlayPickUpSound();
            Destroy(gameObject);
        }
    }
}