using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, I_CanInteract
{
    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private PlayerAnimation m_PlayerAnimation;

    [SerializeField]
    private PowerupActor m_PowerupActor;

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
        IInteractable interactingObject = c.GetComponent<IInteractable>();
        if (interactingObject != null)
        {
            interactingObject.Interact(this);
        }
    }

    public void BoostJump() 
    {
        m_PowerupActor.JumpHigher(JumpPowerUp.JUMP_BUFF_DURATION);
    }

    public void BoostSpeed() 
    {
        m_PowerupActor.GoFaster(SpeedPowerUp.SPEED_BUFF_DURATION);
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
        res.x = m_PowerupActor.IsDoubleSpeed() ? 1.5f : 1.0f;
        res.y = 1;
        res.z = m_PowerupActor.IsDoubleSpeed() ? 1.5f : 1.0f;
        return res;
    }

    private float ComputeJumpHeightModifier()
    {
        return m_PowerupActor.IsDoubleJump() ? 1.5f : 1.0f;
    }

    private float ComputeGravityModifier()
    {
        // Empirical values here
        return m_PowerupActor.IsDoubleJump() ? 0.85f : 1.0f;
    }
}
