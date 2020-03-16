using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(Player))]
public class PlayerNetworkHandler : MonoBehaviour
{
    private PlayerNetworkObject m_PlayerNetworkObject;

    [SerializeField]
    private Animator m_Animator;

    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;

    private void Start()
    {
        m_PlayerNetworkObject = GetComponent<Player>().networkObject;
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (m_PlayerNetworkObject == null)
            return;

        if (m_PlayerNetworkObject.IsOwner)
        {
            if (m_PlayerMovement == null || m_PlayerAnimation == null)
            {
                Debug.LogWarning("Movement/Animation script not found on local player");
                return;
            }

            // Send movement state
            m_PlayerNetworkObject.position = transform.position;

            // Send animation state
            m_PlayerNetworkObject.axisDeltaMagnitude = m_PlayerMovement.GetInputAxis().magnitude;
            m_PlayerNetworkObject.vertVelocity = m_PlayerMovement.GetVelocity().y;
            m_PlayerNetworkObject.rotation = transform.rotation;
            m_PlayerNetworkObject.grounded = m_PlayerMovement.IsGrounded();

            if (m_PlayerMovement.JumpedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_JUMP, Receivers.All);
        }
        else
        {
            // Receive movement state
            transform.position = m_PlayerNetworkObject.position;

            // Receive animation state
            transform.rotation = m_PlayerNetworkObject.rotation;
            m_Animator.SetFloat("Velocity-XZ-Normalized-01", m_PlayerNetworkObject.axisDeltaMagnitude);
            m_Animator.SetFloat("Velocity-Y-Normalized", m_PlayerNetworkObject.vertVelocity);
            m_Animator.SetBool("Grounded", m_PlayerNetworkObject.grounded);
        }
    }

    public void TriggerJump()
    {
        m_Animator.SetTrigger("Jump");
    }
}
