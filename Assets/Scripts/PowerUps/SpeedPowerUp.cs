public class SpeedPowerUp : PowerUp
{
    public const float m_BuffDuration = 5.0f;

    public override void HandlePowerup(PowerupActor powerupActor)
    {
        if (powerupActor != null && !powerupActor.IsDoubleSpeed())
        {
            powerupActor.GoFaster(m_BuffDuration);
        }
    }
}
