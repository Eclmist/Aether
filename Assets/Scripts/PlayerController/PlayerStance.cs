using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStance : MonoBehaviour
{
    public enum Stance
    {
        STANCE_UNARMED   = 0,
        STANCE_TWOHANDED = 1,
        STANCE_ONEHANDED = 2,
    }

    private Stance m_Stance;
    private PlayerMovement m_PlayerMovement;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        SetPlayerRotation();
    }

    void SetPlayerRotation()
    {
        if (m_PlayerMovement.IsDashing())
            return;

        if (m_Stance == Stance.STANCE_UNARMED)
        {
            Vector3 velocity = m_PlayerMovement.GetVelocity();
            velocity.y = 0;
            if (velocity.magnitude > 0.0f)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity.normalized), Time.deltaTime * 10);
        }
        else
        {
            // Look towards camera lookat
            Vector3 cameraLookAt = Camera.main.transform.forward;
            cameraLookAt.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraLookAt.normalized), Time.deltaTime * 10);
        }
    }

    public void SetStance(Stance targetStance)
    {
        if (m_Stance == targetStance)
            return;

        m_Stance = targetStance;
    }

    public Stance GetStance()
    {
        return m_Stance;    
    }

    public bool IsCombatStance()
    {
        return m_Stance != Stance.STANCE_UNARMED;
    }
}

