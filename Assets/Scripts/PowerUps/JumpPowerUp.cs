public class JumpPowerUp : PowerUp
{
    public const float m_BuffDuration = 5.0f;

    public override void HandlePowerup(PowerupActor powerupActor)
    {
        if (powerupActor != null && !powerupActor.IsDoubleJump())
        {
            powerupActor.JumpHigher(m_BuffDuration);
        }
    }
}
