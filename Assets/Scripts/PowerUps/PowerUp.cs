using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IInteractable
{
    protected void PlayPickUpSound() 
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public abstract float GetBuffDuration();

    public void Interact(PlayerHandler interactor) 
    {
        if (interactor != null) 
        {
            Destroy(gameObject);
        }
    }
}
