using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerManager : PlayerManagerBehavior
{
    [SerializeField]
    private Transform[] m_SpawnPositionsRed;

    [SerializeField]
    private Transform[] m_SpawnPositionsBlue;

    private Player m_LocalPlayer;
    private List<Player> m_Players;

    // For host to keep track
    private int m_CurrentPlayerCount = 0;
    private int m_TotalPlayerCount;

    void Awake()
    {
        m_Players = new List<Player>();
        AetherNetworkManager.Instance.sceneChanged += OnSceneLoaded;
    }

    public Player GetLocalPlayerInstance()
    {
        return m_LocalPlayer;
    }

    public List<Player> GetAllPlayers()
    {
        return m_Players;
    }

    public List<Player> GetPlayersByTeam(int teamId)
    {
        return m_Players.FindAll(p => p.GetTeam() == teamId);
    }

    // Called by individual clients to spawn their own player
    private void SpawnOwnPlayer(AetherNetworkManager.PlayerDetails details)
    {
        MainThreadManager.Run(() => {
            Vector3 spawnPosition;
            if (details.teamId == 0)
                spawnPosition = m_SpawnPositionsRed[details.position].position;
            else
                spawnPosition = m_SpawnPositionsBlue[details.position].position;

            Player p = NetworkManager.Instance.InstantiatePlayer(position: spawnPosition) as Player;

            p.playerLoaded += OnPlayerLoaded;
            p.SetTeam(details.teamId);
            m_LocalPlayer = p;
        });
    }

    // Called by individual clients to sync player details
    private void SyncPlayer(uint networkId, int teamId)
    {
        Player[] players = FindObjectsOfType<Player>();
        Player playerToAdd = null;
        foreach (Player p in players)
        {
            if (p.networkObject.NetworkId == networkId)
            {
                playerToAdd = p;
                break;
            }
        }

        if (playerToAdd == null)
        {
            Debug.LogWarning("No such player found");
            return;
        }

        playerToAdd.SetTeam(teamId);
        m_Players.Add(playerToAdd);
    }

    // Called by individual clients to setup the revealactors
    private void SetupRevealActors()
    {
        int myTeamId = m_LocalPlayer.GetTeam();
        foreach (Player p in m_Players)
            p.GetRevealActor().SetIsRevealer(p.GetTeam() == myTeamId);
    }

    // Callback for when each player has finished setting up network
    public void OnPlayerLoaded(Player player)
    {
        if (!player.networkObject.IsOwner)
        {
            Debug.Log("Should only be called by owner");
            return;
        }

        // Request host to sync this player to everyone
        networkObject.SendRpc(RPC_REQUEST_PLAYER_SYNC, Receivers.Server, player.networkObject.NetworkId, player.GetTeam());
    }

    private static void DestroyPlayer(PlayerBehavior player)
    {
        Destroy(player);
    }


    ////////////////////
    ///
    /// CLIENT-ONLY CODE
    ///
    ////////////////////

    // RPC sent from host to trigger the spawning of each client's own player.
    public override void TriggerSetupPlayer(RpcArgs args)
    {
        AetherNetworkManager.PlayerDetails details = new AetherNetworkManager.PlayerDetails();
        details.teamId = args.GetNext<int>();
        details.position = args.GetNext<int>();

        SpawnOwnPlayer(details);
    }

    // RPC sent from host to trigger player sync
    public override void TriggerPlayerSync(RpcArgs args)
    {
        uint networkId = args.GetNext<uint>();
        int teamId = args.GetNext<int>();
        SyncPlayer(networkId, teamId);
    }

    // RPC sent from host to trigger setting up of reveal system
    public override void TriggerSetupReveal(RpcArgs args)
    {
        SetupRevealActors();
    }


    ////////////////////
    ///
    /// HOST-ONLY CODE
    ///
    ////////////////////

    // RPC sent from each player(including host himself) to request for sync
    public override void RequestPlayerSync(RpcArgs args)
    {
        uint networkId = args.GetNext<uint>();
        int teamId = args.GetNext<int>();

        // Update host
        SyncPlayer(networkId, teamId);

        // Update clients
        NetworkManager.Instance.Networker.IteratePlayers(np =>
        {
            if (np == NetworkManager.Instance.Networker.Me)
                return;

            networkObject.SendRpc(np, RPC_TRIGGER_PLAYER_SYNC, networkId, teamId);
        });

        // Host received all players, set up reveal system
        m_CurrentPlayerCount++;
        Debug.Log("Current Player Count: " + m_CurrentPlayerCount);
        if (m_CurrentPlayerCount == m_TotalPlayerCount)
        {
            SetupRevealActors();
            networkObject.SendRpc(RPC_TRIGGER_SETUP_REVEAL, Receivers.All);
        }
    }

    // Callback for when scene has loaded for all players
    public void OnSceneLoaded(Dictionary<NetworkingPlayer, AetherNetworkManager.PlayerDetails> detailsMap)
    {
        if (!networkObject.IsServer)
            return;

        // Host spawns his own player
        NetworkingPlayer npSelf = NetworkManager.Instance.Networker.Me;
        SpawnOwnPlayer(detailsMap[npSelf]);

        // Trigger other clients to spawn their own players
        NetworkManager.Instance.Networker.IteratePlayers(np =>
        {
            if (np == npSelf)
                return;

            AetherNetworkManager.PlayerDetails details = detailsMap[np];

            networkObject.SendRpc(np, RPC_TRIGGER_SETUP_PLAYER, details.teamId, details.position);
        });

        m_TotalPlayerCount = detailsMap.Count;
        Debug.Log(m_TotalPlayerCount + " Players");
    }
}
