using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(ClientServerTogglables))]
public class PlayerNetworkHandler : PlayerBehavior
{
    private ClientServerTogglables m_ClientServerTogglables;

    protected override void NetworkStart() {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
    }

    void Awake()
    {
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
    }
}
