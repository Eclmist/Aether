using System.Collections;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    private const int m_Id = 1;

    [SerializeField]
    private const float m_SpeedModifier = 1.50f;

    public override void HandlePowerup(PowerupHandler powerupHandler, PlayerMovement playerMovement)
    {
        if (!powerupHandler.IsBoosted(m_Id))
        {
            OnPowerupActivated(playerMovement);
            powerupHandler.StartCoroutine(StartPowerup(powerupHandler, playerMovement));
        }
    }

    protected override IEnumerator StartPowerup(PowerupHandler powerupHandler, PlayerMovement playerMovement)
    {
        powerupHandler.SetBoosted(m_Id, true);
        yield return new WaitForSeconds(m_BuffDuration);
        OnPowerupExpired(playerMovement);
        powerupHandler.SetBoosted(m_Id, false);
    }

    public override void OnPowerupExpired(PlayerMovement playerMovement)
    {
        playerMovement.SetExternalVelocityModifier(Vector3.one);
    }

    public override void OnPowerupActivated(PlayerMovement playerMovement)
    {
        playerMovement.SetExternalVelocityModifier(ComputeVelocityModifier());
    }

    private Vector3 ComputeVelocityModifier()
    {
        Vector3 res = new Vector3(m_SpeedModifier, 1.0f, m_SpeedModifier);
        return res;
    }
}
