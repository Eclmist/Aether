using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerNetworkManager : PlayerNetworkManagerBehavior
{
    public event System.Action AllPlayersReady;

    [SerializeField]
    private Transform[] m_SpawnPositionsRed;

    [SerializeField]
    private Transform[] m_SpawnPositionsBlue;

    private int m_ReadyClientCount = 0;

    private void Awake()
    {
        AetherNetworkManager.Instance.SceneLoaded += OnSceneLoaded;
        PlayerManager.Instance.PlayerListPopulated += OnClientReady;
    }

    private void OnClientReady()
    {
        // Run on main thread to lock data and send rpc
        MainThreadManager.Run(() =>
        {
            List<Player> players = PlayerManager.Instance.GetAllPlayers();

            Player localPlayer = PlayerManager.Instance.GetLocalPlayer();
            if (localPlayer == null)
                Debug.LogError("Local player not set");

            PlayerDetails localPlayerDetails = localPlayer.GetPlayerDetails();
            foreach (Player player in players)
            {
                // Remove unnecessary objects/components
                player.UpdateToggleables();

                // Set reveal based on team
                // Player details not null
                PlayerDetails details = player.GetPlayerDetails();
                RevealActor revealActor = player.GetRevealActor();
                if (details.GetTeam().Equals(localPlayerDetails.GetTeam()))
                    revealActor.SetRevealMode(RevealActor.RevealMode.REVEALMODE_SHOW);
                else
                    revealActor.SetRevealMode(RevealActor.RevealMode.REVEALMODE_HIDE);

                revealActor.enabled = true;
            }

            networkObject.SendRpc(RPC_SET_CLIENT_READY, Receivers.Server);

            // Unsubscribe from playersloaded
            PlayerManager.Instance.PlayerListPopulated -= OnClientReady;
        });
    }

    public override void SetPlayerCount(RpcArgs args)
    {
        // Run on main thread to lock player count
        MainThreadManager.Run(() =>
        {
            PlayerManager.Instance.SetPlayerCount(args.GetNext<int>());
        });
    }

    public override void SetAllReady(RpcArgs args)
    {
        AllPlayersReady?.Invoke();
    }

    ////////////////////
    ///
    /// HOST-ONLY CODE
    ///
    ////////////////////

    // Callback for when scene has loaded for all players
    public void OnSceneLoaded(Dictionary<NetworkingPlayer, PlayerDetails> detailsMap)
    {
        if (!networkObject.IsServer)
            return;

        // Run on main thread to send RPC.
        MainThreadManager.Run(() =>
        {
            // Set total player count
            networkObject.SendRpc(RPC_SET_PLAYER_COUNT, Receivers.All, detailsMap.Count);
        });

        // Host spawns all players
        NetworkManager.Instance.Networker.IteratePlayers(np =>
        {
            SpawnPlayer(np, detailsMap[np]);
        });

        AetherNetworkManager.Instance.SceneLoaded -= OnSceneLoaded;
    }

    // Called by host to spawn every client's player
    private void SpawnPlayer(NetworkingPlayer np, PlayerDetails details)
    {
        // Run on main thread for unity to be able to grab transform data and instantiate player etc
        MainThreadManager.Run(() =>
        {
            Transform spawnPoint = null;
            switch(details.GetTeam())
            {
                case Team.TEAM_ONE:
                    spawnPoint = m_SpawnPositionsRed[details.GetPosition()];
                    break;
                case Team.TEAM_TWO:
                    spawnPoint = m_SpawnPositionsBlue[details.GetPosition()];
                    break;
                default:
                    Debug.Assert(false, "Should not be reached unless a team was unhandled. PlayerNetworkManager.SpawnPlayer");
                    break;
            }

            Player p = NetworkManager.Instance.InstantiatePlayer(position: spawnPoint.position, rotation: spawnPoint.rotation) as Player;

            p.SetDetails(details);
            p.networkStarted += OnPlayerNetworked;
        });
    }

    // Callback for when each player's networkobject is set up on the host
    private void OnPlayerNetworked(NetworkBehavior networkBehavior)
    {
        Player player = networkBehavior as Player;
        PlayerDetails details = player.GetPlayerDetails();

        // Update player details
        NetworkingPlayer np = networkObject.Networker.GetPlayerById(details.GetNetworkId());
        player.networkObject.AssignOwnership(np);
        player.networkObject.SendRpc(Player.RPC_TRIGGER_UPDATE_DETAILS, Receivers.All, details.ToArray());

        player.networkStarted -= OnPlayerNetworked;
    }

    // RPC sent to host when a client is ready
    public override void SetClientReady(RpcArgs args)
    {
        if (!networkObject.IsServer)
            return;

        // Run on main thread to ensure client count is locked and rpc can be sent
        MainThreadManager.Run(() =>
        {
            m_ReadyClientCount++;
            if (m_ReadyClientCount == PlayerManager.Instance.GetPlayerCount())
            {
                // Tell clients that all players are ready
                networkObject.SendRpc(RPC_SET_ALL_READY, Receivers.All);
            }
        });
    }
}
