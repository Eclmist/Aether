using UnityEngine;

[DisallowMultipleComponent]
public class JumpPowerUp : PowerUpBase
{
    [SerializeField]
    private const float m_JumpHeightModifier = 1.50f;

    [SerializeField]
    private const float m_GravityModifier = 0.85f;

    public override void OnPowerUpExpired()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetExternalJumpHeightModifier(1.0f);
            playerMovement.SetExternalGravityModifier(1.0f);
        }
    }

    public override void OnPowerUpActivated()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            UIManager.Instance.ActivatePowerupIcon(UIPowerUpSignals.JUMP_SIGNAL);
            UIManager.Instance.ModifyHealthBar(0.1f);
            playerMovement.SetExternalJumpHeightModifier(m_JumpHeightModifier);
            playerMovement.SetExternalGravityModifier(m_GravityModifier);
        }
    }
}
