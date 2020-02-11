using System.Collections;
using UnityEngine;

public class JumpPowerUp : PowerUp
{
    private const int m_Id = 0;
    [SerializeField]
    private const float m_JumpHeightModifier = 1.50f;

    [SerializeField]
    private const float m_GravityModifier = 0.85f;

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
        playerMovement.SetExternalJumpHeightModifier(1.0f);
        playerMovement.SetExternalGravityModifier(1.0f);
    }

    public override void OnPowerupActivated(PlayerMovement playerMovement)
    {
        playerMovement.SetExternalJumpHeightModifier(m_JumpHeightModifier);
        playerMovement.SetExternalGravityModifier(m_GravityModifier);
    }
}
