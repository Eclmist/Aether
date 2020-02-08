using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : PowerUp
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
        if (interactor != null && !interactor.GetDoubleJump())
        {
            interactor.JumpHigher();
            PlayPickUpSound();
            Destroy(gameObject);
        }
    }

}