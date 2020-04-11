using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private CharacterCustomizer m_CharacterCustomizerLocal;

    private Dictionary<NetworkingPlayer, LobbyPlayer> m_LobbyPlayers;

    void Awake()
    {
        m_LobbyPlayers = new Dictionary<NetworkingPlayer, LobbyPlayer>();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        // This client is accepted and has joined the lobby. (Includes host itself)
        networkObject.SendRpc(RPC_SET_PLAYER_ENTERED, Receivers.Server, m_CharacterCustomizerLocal.GetDataPacked());
    }

    private bool CanStart()
    {
        // bypass for testing
        if (m_BypassTeamCheck)
            return true;

        if (m_LobbyPlayers.Count != AetherNetworkManager.MAX_PLAYER_COUNT)
            return false;

        int balance = 0;
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            // player not ready, cannot start
            if (!p.GetIsReady())
                return false;

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

    public void OnStart()
    {
        if (!NetworkManager.Instance.IsServer)
            return;

        if (!CanStart())
            return;

        int[] teamList = new int[System.Enum.GetNames(typeof(Team)).Length];
        foreach (NetworkingPlayer np in m_LobbyPlayers.Keys)
        {
            LobbyPlayer lp = m_LobbyPlayers[np];
            Team team = lp.GetTeam();
            int position = teamList[(int)team]++;

            PlayerDetails details = new PlayerDetails(
                np.NetworkId,
                team,
                position,
                lp.GetCustomization()
            ); ;

            AetherNetworkManager.Instance.AddPlayer(np, details);
        }

        AetherNetworkManager.Instance.LoadGame(3);
    }

    private void SetupPlayer(NetworkingPlayer np, ulong customization)
    {
        MainThreadManager.Run(() => {
            int playerCount = m_LobbyPlayers.Count;
            Vector3 position = m_PlayerPositions[playerCount].position;
            Quaternion rotation = m_PlayerPositions[playerCount].rotation;
            Destroy(m_Loaders[playerCount]);

            LobbyPlayer player = NetworkManager.Instance.InstantiateLobbyPlayer(rotation: rotation) as LobbyPlayer;
            player.SetCustomization(customization);
            // TODO: FIX THIS. ONLY HAPPENS LOCALLY.
            player.transform.SetParent(m_PlayerPositions[playerCount], false);

            // Name setup
            string playerName = "Player-" + np.NetworkId;
            player.UpdateName(playerName);

            // Team setup, alternates based on join order by default
            player.UpdateTeam(playerCount % 2 == 0 ? Team.TEAM_ONE : Team.TEAM_TWO);

            m_LobbyPlayers.Add(np, player);
        });
    }

    ////////////////////
    ///
    /// CLIENT-ONLY CODE
    ///
    ////////////////////

    public void SendCustomizationDataToHost()
    {
        networkObject?.SendRpc(RPC_SET_PLAYER_CUSTOMIZATION, Receivers.Server, m_CharacterCustomizerLocal.GetDataPacked());
    }

    // RPC sent to host by any player
    public override void ToggleReady(RpcArgs args)
    {
        NetworkingPlayer np = args.Info.SendingPlayer;
        LobbyPlayer lobbyPlayer;
        if (m_LobbyPlayers.TryGetValue(np, out lobbyPlayer))
            lobbyPlayer.ToggleReadyStatus();
    }

    // RPC sent to host by new entering player
    public override void SetPlayerEntered(RpcArgs args)
    {
        NetworkingPlayer np = args.Info.SendingPlayer;
        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            // Send current lobby data to new player
            p.UpdateDataFor(np);
        }

        SetupPlayer(np, args.GetNext<ulong>());
    }

    public override void SetPlayerCustomization(RpcArgs args)
    {
        NetworkingPlayer np = args.Info.SendingPlayer;
        m_LobbyPlayers[np].SetCustomization(args.GetNext<ulong>());
    }
}
