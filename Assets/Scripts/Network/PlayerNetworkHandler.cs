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

    private void Start()
    {
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
        m_ClientServerTogglables.Init();
    }

    void Update() {
        if (networkObject != null && !networkObject.IsOwner) {
            // put here for convenience, rather than an additional network movement class
            transform.position = networkObject.position;
        }
    }
}
