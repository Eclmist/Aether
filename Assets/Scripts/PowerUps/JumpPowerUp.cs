using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : PowerUp
{
    public const float m_BuffDuration = 5.0f;

    void OnTriggerEnter(Collider c)
    {
        PlayerHandler interactor = c.GetComponent<PlayerHandler>();
        Interact(interactor);
    }

    public override void HandlePowerups(PowerupActor powerupActor)
    {
        if (powerupActor != null && !powerupActor.IsDoubleJump())
        {
            powerupActor.JumpHigher(m_BuffDuration);
        }
    }
}