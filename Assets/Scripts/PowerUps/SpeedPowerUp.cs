using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public static float SPEED_BUFF_DURATION = 5.0f;

    void OnTriggerEnter(Collider c)
    {
        PlayerHandler interactor = c.GetComponent<PlayerHandler>();
        Interact(interactor);
    }

    public void Interact(ICanInteract interactor) 
    {
        if (interactor != null && interactor is PlayerHandler) 
        {
            PlayPickUpSound();
            ((PlayerHandler) interactor).BoostSpeed();
            Destroy(gameObject);
        }
    }
}