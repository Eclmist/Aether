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
    private Transform[] m_PlayerPositions;

    [SerializeField]
    private GameObject[] m_Loaders;

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

            LobbyPlayer player = NetworkManager.Instance.InstantiateLobbyPlayer(rotation : rotation) as LobbyPlayer;
            player.transform.SetParent(m_PlayerPositions[m_PlayerCount].transform, false);

            // Name setup
            string playerName = "Player-" + np.NetworkId;
            player.UpdateName(playerName);

            // Team setup
            player.UpdateTeam(m_PlayerCount < 1 ? Team.TEAM_ONE : Team.TEAM_TWO);

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
        if (m_PlayerCount != AetherNetworkManager.MAX_PLAYER_COUNT)
            return false;

        int balance = 0;
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            switch (p.GetTeam())
            {
                case Team.TEAM_ONE:
                    balance++;
                    break;
                case Team.TEAM_TWO:
                    balance--;
                    break;
                default:
                    Debug.Assert(false, "Should not be reached unless a team was unhandled. LobbySystem.CanStart");
                    break;
            }
        }

        return balance == 0;
    }

    private void OnPlayerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
            p.UpdateDataFor(player);
   
        SetupPlayer(player);
    }

    public void OnStart()
    {
        if (NetworkManager.Instance.IsServer)
        {
            if (CanStart())
            {
                int teamOne = 0;
                int teamTwo = 0;
                foreach (NetworkingPlayer np in m_LobbyPlayers.Keys)
                {
                    Team team = m_LobbyPlayers[np].GetTeam();
                    int position = 0;
                    switch (team)
                    {
                        case Team.TEAM_ONE:
                            position = teamOne++;
                            break;
                        case Team.TEAM_TWO:
                            position = teamTwo++;
                            break;
                        default:
                            Debug.Assert(false, "Should not be reached unless a team was unhandled. LobbySystem.CanStart");
                            break;
                    }

                    PlayerDetails details = new PlayerDetails(
                        np.NetworkId,
                        team,
                        position
                    );

                    AetherNetworkManager.Instance.AddPlayer(np, details);
                }

                AetherNetworkManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.Networker.playerAccepted -= OnPlayerAccepted;
    }
}
