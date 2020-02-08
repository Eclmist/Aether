using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    void OnTriggerEnter(Collider c)
    {
        PlayerHandler interactor = c.GetComponent<PlayerHandler>();
        BuffInteractor(interactor);
    }

    public override void BuffInteractor(PlayerHandler interactor)
    {
        if (interactor != null)
        {
            PlayPickUpSound();
            interactor.HandleInteraction(this);
            Destroy(gameObject);
        }
    }
}