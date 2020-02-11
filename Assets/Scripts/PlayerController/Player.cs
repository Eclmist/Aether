﻿using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(ClientServerTogglables))]
public class Player : PlayerBehavior, ICanInteract
{
    private PlayerMovement m_PlayerMovement;

    private PlayerAnimation m_PlayerAnimation;

    private ClientServerTogglables m_ClientServerTogglables;

    private List<PowerUp> m_PowerupsList;

    void Awake()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();

        m_PowerupsList = new List<PowerUp>();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;
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

    private void InteractWith(IInteractable interactable)
    {
        if (interactable != null) // null check done here instead. 
        {
            interactable.Interact(this);
        }
    }
}
