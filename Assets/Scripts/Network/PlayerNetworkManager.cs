using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class PlayerNetworkManager : PlayerNetworkManagerBehavior
{
    [SerializeField]
    private Transform[] m_SpawnPositionsRed;

    [SerializeField]
    private Transform[] m_SpawnPositionsBlue;

    void Awake()
    {
        AetherNetworkManager.Instance.SceneChanged += OnSceneLoaded;
        PlayerManager.Instance.PlayersLoaded += OnPlayersLoaded;
    }

    private void OnPlayersLoaded()
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
            if (details.GetTeam() == localPlayerDetails.GetTeam())
                revealActor.SetRevealMode(RevealActor.RevealMode.REVEALMODE_SHOW);
            else
                revealActor.SetRevealMode(RevealActor.RevealMode.REVEALMODE_HIDE);

            revealActor.enabled = true;
        }
    }

    public override void SetPlayerCount(RpcArgs args)
    {
        PlayerManager.Instance.SetPlayerCount(args.GetNext<int>());
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

        // Set total player count
        PlayerManager.Instance.SetPlayerCount(detailsMap.Count);
        networkObject.SendRpc(RPC_SET_PLAYER_COUNT, Receivers.All, detailsMap.Count);

        // Host spawns all players
        NetworkManager.Instance.Networker.IteratePlayers(np =>
        {
            SpawnPlayer(np, detailsMap[np]);
        });

    }

    // Called by host to spawn every client's player
    private void SpawnPlayer(NetworkingPlayer np, PlayerDetails details)
    {
        MainThreadManager.Run(() => {
            Transform spawnPoint;
            if (details.GetTeam() == 0)
                spawnPoint = m_SpawnPositionsRed[details.GetPosition()];
            else
                spawnPoint = m_SpawnPositionsBlue[details.GetPosition()];

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
    }
}
