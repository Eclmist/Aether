using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimation), typeof(ClientServerTogglables))]
public class Player : PlayerBehavior, ICanInteract
{
    private PlayerMovement m_PlayerMovement;

    private PlayerAnimation m_PlayerAnimation;

    private PowerupActor m_PowerupActor;

    private ClientServerTogglables m_ClientServerTogglables;

    private const float m_SpeedModifier = 1.50f;

    private const float m_JumpModifier = 1.50f; 

    private const float m_GravityModifier = 0.85f;

    void Awake()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_PowerupActor = GetComponent<PowerupActor>();
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;
    }

    void Update()
    {
        UpdateVelocity();
    }

    void OnTriggerEnter(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>());
    }

    void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }

    public PlayerMovement GetPlayerMovement()
    {
        return m_PlayerMovement;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        m_PlayerMovement = playerMovement;
    }

    public PowerupActor GetPlayerPowerupActor()
    {
        return m_PowerupActor;
    }

    public void SetPlayerPowerupActor(PowerupActor powerupActor)
    {
        m_PowerupActor = powerupActor;
    }

    private void InteractWith(IInteractable interactable)
    {
        if (interactable != null) // null check done here instead. 
        {
            interactable.Interact(this);
        }
    }

    private void UpdateVelocity()
    {
        m_PlayerMovement.SetExternalVelocityModifier(ComputeVelocityModifier());
        m_PlayerMovement.SetExternalJumpHeightModifier(ComputeJumpHeightModifier());
        m_PlayerMovement.SetExternalGravityModifier(ComputeGravityModifier());
    }

    private Vector3 ComputeVelocityModifier()
    {
        Vector3 res = Vector3.zero;
        res.x = m_PowerupActor.IsDoubleSpeed() ? m_SpeedModifier : 1.0f;
        res.y = 1;
        res.z = m_PowerupActor.IsDoubleSpeed() ? m_SpeedModifier : 1.0f;
        return res;
    }

    private float ComputeJumpHeightModifier()
    {
        return m_PowerupActor.IsDoubleJump() ? m_JumpModifier : 1.0f;
    }

    private float ComputeGravityModifier()
    {
        // Empirical values here
        return m_PowerupActor.IsDoubleJump() ? m_GravityModifier : 1.0f;
    }
}
