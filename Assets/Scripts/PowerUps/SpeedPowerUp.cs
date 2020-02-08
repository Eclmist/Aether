using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    private const float m_BuffDuration = 5.0f;

    void OnTriggerEnter(Collider c)
    {
        PlayerHandler interactor = c.GetComponent<PlayerHandler>();
        Interact(interactor);
    }

    public override float GetBuffDuration()
    {
        return m_BuffDuration;
    }

    public void Interact(PlayerHandler interactor) 
    {
        if (interactor != null) 
        {
            PlayPickUpSound();
            interactor.HandleInteraction(this);
            Destroy(gameObject);
        }
    }
}