using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PowerupActor))]
public class VelocityModifier : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;
    private PowerupActor m_PowerupActor;

    // Start is called before the first frame update
    void Start()
    {
        // Null guards not necessary here because the components are guaranteed to exist
        // due to the RequireComponent property at the top of the class.
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PowerupActor = GetComponent<PowerupActor>();
    }

    private void Update()
    {
        m_PlayerMovement.SetExternalVelocityModifier(ComputeVelocityModifier());
        m_PlayerMovement.SetExternalJumpHeightModifier(ComputeJumpHeightModifier());
        m_PlayerMovement.SetExternalGravityModifier(ComputeGravityModifier());
    }

    private Vector3 ComputeVelocityModifier()
    {
        Vector3 res = Vector3.zero;
        res.x = m_PowerupActor.GetDoubleSpeed() ? 2.0f : 1.0f;
        res.y = 1;
        res.z = m_PowerupActor.GetDoubleSpeed() ? 2.0f : 1.0f;
        return res;
    }

    private float ComputeJumpHeightModifier()
    {
        return m_PowerupActor.GetDoubleJump() ? 2.0f : 1.0f;
    }

    private float ComputeGravityModifier()
    {
        // Empirical values here
        return m_PowerupActor.GetDoubleJump() ? 0.75f : 1.0f;
    }
}
