using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, IInteractor
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

    public void HandleInteraction(IInteractable interactable) {
        if (interactable is JumpPowerUp) {
            m_PowerupActor.JumpHigher();
        } 

        if (interactable is SpeedPowerUp) {
            m_PowerupActor.GoFaster();
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
        res.x = m_PowerupActor.GetDoubleSpeed() ? 1.5f : 1.0f;
        res.y = 1;
        res.z = m_PowerupActor.GetDoubleSpeed() ? 1.5f : 1.0f;
        return res;
    }

    private float ComputeJumpHeightModifier()
    {
        return m_PowerupActor.GetDoubleJump() ? 1.5f : 1.0f;
    }

    private float ComputeGravityModifier()
    {
        // Empirical values here
        return m_PowerupActor.GetDoubleJump() ? 0.85f : 1.0f;
    }
}
