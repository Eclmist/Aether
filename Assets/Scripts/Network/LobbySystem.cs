using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbySystem : LobbySystemBehavior
{
    [SerializeField]
    private bool m_BypassTeamCheck;

    [SerializeField]
    private List<Transform> m_PlayerPositions;

    [SerializeField]
    private List<GameObject> m_Loaders;

    private Dictionary<NetworkingPlayer, LobbyPlayer> m_LobbyPlayers;

    private int m_PlayerCount;

    void Awake()
    {
        m_LobbyPlayers = new Dictionary<NetworkingPlayer, LobbyPlayer>();
        m_PlayerCount = 0;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        if (!networkObject.IsServer)
            return;

        // Setup host
        SetupPlayer(networkObject.Networker.Me);

        NetworkManager.Instance.Networker.playerAccepted += OnPlayerAccepted;
    }

    private void SetupPlayer(NetworkingPlayer np)
    {
        MainThreadManager.Run(() => {
            Vector3 position = m_PlayerPositions[m_PlayerCount].position;
            Quaternion rotation = m_PlayerPositions[m_PlayerCount].rotation;
            Destroy(m_Loaders[m_PlayerCount]);

            LobbyPlayer player = (LobbyPlayer)NetworkManager.Instance.InstantiateLobbyPlayer(rotation : rotation);
            player.transform.SetParent(m_PlayerPositions[m_PlayerCount].transform, false);

            // Name setup
            string playerName = "Player-" + np.NetworkId;
            player.UpdateName(playerName);

            // Team setup
            player.UpdateTeam(m_PlayerCount % 2);

            m_LobbyPlayers.Add(np, player);
            m_PlayerCount++;
        });
    }

    private bool CanStart()
    {
        // bypass for testing
        if (m_BypassTeamCheck)
            return true;

        // TODO: Add ready check
        if (m_PlayerCount != 4)
            return false;

        int balance = 0;
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            if (p.Team == 0)
                balance++;
            else
                balance--;
        }

        return balance == 0;
    }

    private void OnPlayerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            p.UpdateDataFor(player);
        }
   
        SetupPlayer(player);
    }

    public void OnStart()
    {
        if (NetworkManager.Instance.IsServer)
        {
            if (CanStart())
            {
                int left = 0;
                int right = 0;
                foreach (NetworkingPlayer np in m_LobbyPlayers.Keys)
                {
                    AetherNetworkManager.PlayerDetails details;
                    details.team = m_LobbyPlayers[np].Team;
                    details.position = details.team == 0 ? left++ : right++;

                    AetherNetworkManager.Instance.AddPlayer(np, details);
                }

                AetherNetworkManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
