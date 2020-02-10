using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimation), typeof(PowerupActor))]
public class PlayerHandler : MonoBehaviour, ICanInteract
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private PlayerAnimation m_PlayerAnimation;

    [SerializeField]
    private PowerupActor m_PowerupActor;

    private const float m_SpeedModifier = 1.50f;

    private const float m_JumpModifier = 1.50f; 

    private const float m_GravityModifier = 0.85f;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PowerupActor = GetComponent<PowerupActor>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
    }

    void OnTriggerEnter(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>());
    }

    private void InteractWith(IInteractable interactable) 
    {
        if (interactable != null) // null check done here instead. 
        {
            interactingObject.Interact(this);
        }
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
