using UnityEngine;

[DisallowMultipleComponent]
public class SpeedPowerUp : PowerUpBase
{
    [SerializeField]
    private const float m_SpeedModifier = 1.50f;


    public override void OnPowerUpExpired()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetExternalVelocityModifier(Vector3.one);
        }
    }

    public override void OnPowerUpActivated()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetExternalVelocityModifier(ComputeVelocityModifier());
        }
    }

    private Vector3 ComputeVelocityModifier()
    {
        Vector3 res = new Vector3(m_SpeedModifier, 1.0f, m_SpeedModifier);
        return res;
    }
}
