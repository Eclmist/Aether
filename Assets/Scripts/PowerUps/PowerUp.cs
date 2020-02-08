using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IInteractable
{
    public void Interact(PowerupActor interactor) 
    {
        if (interactor != null) 
        {
            Destroy(gameObject);
        }
    }

    protected void PlayPickUpSound() 
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public abstract void BuffInteractor(PowerupActor interactor);

    
}
