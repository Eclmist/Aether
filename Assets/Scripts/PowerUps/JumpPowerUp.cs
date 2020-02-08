using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : PowerUp
{
    public static float JUMP_BUFF_DURATION = 5.0f;

    void OnTriggerEnter(Collider c)
    {
        PlayerHandler interactor = c.GetComponent<PlayerHandler>();
        Interact(interactor);
    }

    public void Interact(I_CanInteract interactor) 
    {
        if (interactor != null && interactor is PlayerHandler) 
        {
            PlayPickUpSound();
            ((PlayerHandler) interactor).BoostJump();
            Destroy(gameObject);
        }
    }
}