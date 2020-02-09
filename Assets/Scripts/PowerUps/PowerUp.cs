using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IInteractable
{
    private void PlayPickUpSound() 
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public void Interact(ICanInteract interactor) 
    {
        if (interactor != null && interactor is PlayerHandler) 
        {
            PlayPickUpSound();
            HandlePowerups(((PlayerHandler) interactor).GetPlayerPowerupActor());
            Destroy(gameObject);
        }
    }

    public abstract void HandlePowerups(PowerupActor powerupActor);
    
}
