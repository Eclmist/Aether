using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(ClientServerTogglables))]
public class PlayerNetworkHandler : PlayerBehavior
{
    private ClientServerTogglables m_ClientServerTogglables;

    void Awake()
    {
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
    }

    protected override void NetworkStart() {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;
    }

    void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }
}
