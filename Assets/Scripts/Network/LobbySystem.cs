using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbySystem : LobbySystemBehavior
{

    [SerializeField]
    private List<Transform> m_PlayerPositions;

    private List<LobbyPlayer> m_LobbyPlayers;

    private int m_PlayerCount;

    void Awake()
    {
        m_LobbyPlayers = new List<LobbyPlayer>();
        m_PlayerCount = 0;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        if (!networkObject.IsServer)
            return;

        // Setup host
        SetupPlayer(networkObject.Networker.Me.NetworkId);

        NetworkManager.Instance.Networker.playerAccepted += PlayerAccepted;
    }

    private void PlayerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        foreach (LobbyPlayer p in m_LobbyPlayers)
        {
            p.UpdateNameFor(player);
        }
        SetupPlayer(player.NetworkId);
    }

    private void SetupPlayer(uint playerId)
    {
        MainThreadManager.Run(() => {
            Vector3 position = m_PlayerPositions[m_PlayerCount].position;
            LobbyPlayer player = (LobbyPlayer)NetworkManager.Instance.InstantiateLobbyPlayer(position: position);

            m_LobbyPlayers.Add(player);
            m_PlayerCount++;

            // Name setup
            string playerName = "Player-" + playerId;
            player.UpdateName(playerName);
        });
    }
}
