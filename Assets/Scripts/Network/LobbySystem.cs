using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbySystem : LobbySystemBehavior
{
    [SerializeField]
    private bool m_BypassReadyCheck;

    [SerializeField]
    private Transform[] m_PlayerPositions;

    [SerializeField]
    private GameObject[] m_Loaders;

    [SerializeField]
    private CharacterCustomizer m_CharacterCustomizerLocal;

    private Dictionary<NetworkingPlayer, LobbyPlayer> m_LobbyPlayers;

    private void Awake()
    {
        m_LobbyPlayers = new Dictionary<NetworkingPlayer, LobbyPlayer>();
        if (NetworkManager.Instance.IsServer)
            NetworkManager.Instance.InstantiateAether();
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
        if (m_BypassReadyCheck)
            return true;

        foreach (LobbyPlayer p in m_LobbyPlayers.Values)
        {
            // player not ready, cannot start
            if (!p.GetIsReady())
                return false;
        }

        return true;
    }

    ////////////////////
    ///
    /// HOST-ONLY CODE
    ///
    ////////////////////
    
    public void OnStart()
    {
        if (!NetworkManager.Instance.IsServer)
            return;

        if (!CanStart())
            return;

        int position = 0;
        foreach (NetworkingPlayer np in m_LobbyPlayers.Keys)
        {
            LobbyPlayer lp = m_LobbyPlayers[np];

            PlayerDetails details = new PlayerDetails(
                np.NetworkId,
                position++,
                lp.GetCustomization()
            );

            AetherNetworkManager.Instance.AddPlayer(np, details);
        }

        AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.KOTH_SCENE_INDEX);
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
            lobbyPlayer.ToggleReadyStatus(args.GetNext<bool>());
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
