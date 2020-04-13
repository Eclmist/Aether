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

    // Singleton instance
    private static LobbySystem m_Instance;

    // Host only
    private Dictionary<NetworkingPlayer, LobbyPlayer> m_LobbyPlayers;

    private void Awake()
    {
        // Ensure only one of this class exists
        if (m_Instance != null)
            Destroy(m_Instance);
        m_Instance = this;

        m_LobbyPlayers = new Dictionary<NetworkingPlayer, LobbyPlayer>();
        if (NetworkManager.Instance.IsServer)
            NetworkManager.Instance.InstantiateAether();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        // This client is accepted and has joined the lobby. (Includes host itself)
        string name = PlayerPrefs.GetString("nickname", "Guest");
        ulong customization = m_CharacterCustomizerLocal.GetDataPacked();
        networkObject.SendRpc(RPC_SET_PLAYER_ENTERED, Receivers.Server, name, customization);
    }

    public bool CanStart()
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

    public void SetPlayerInPosition(LobbyPlayer player)
    {
        int index = player.GetPosition();
        if (index >= AetherNetworkManager.MAX_PLAYER_COUNT)
            return;

        Destroy(m_Loaders[index]);
        Transform parent = m_PlayerPositions[index];
        player.transform.SetParent(parent, false);
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

        int position = 0;
        foreach (NetworkingPlayer np in m_LobbyPlayers.Keys)
        {
            LobbyPlayer lp = m_LobbyPlayers[np];

            PlayerDetails details = new PlayerDetails(
                lp.GetName(),
                np.NetworkId,
                position++,
                lp.GetCustomization()
            );

            AetherNetworkManager.Instance.AddPlayer(np, details);
        }

        AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.KOTH_SCENE_INDEX);
    }

    private void SetupPlayer(NetworkingPlayer np, string name, ulong customization)
    {
        MainThreadManager.Run(() => {
            int playerCount = m_LobbyPlayers.Count;

            LobbyPlayer player = NetworkManager.Instance.InstantiateLobbyPlayer() as LobbyPlayer;

            player.networkStarted += (NetworkBehavior behavior) =>
            {
                // Position setup
                player.UpdatePosition(playerCount);

                // Customization setup
                player.SetCustomization(customization);

                // Name setup
                player.UpdateName(name);
            };

            m_LobbyPlayers.Add(np, player);
        });
    }

    ////////////////////
    ///
    /// CLIENT-ONLY CODE
    ///
    ////////////////////
    
    public void SetPlayerReady(bool isReady)
    {
        networkObject?.SendRpc(RPC_TOGGLE_READY, Receivers.Server, isReady);
    }

    public void SendCustomizationDataToHost()
    {
        networkObject?.SendRpc(RPC_SET_PLAYER_CUSTOMIZATION, Receivers.Server, m_CharacterCustomizerLocal.GetDataPacked());
    }

    // RPC sent to host by any player
    public override void ToggleReady(RpcArgs args)
    {
        MainThreadManager.Run(() =>
        {
            NetworkingPlayer np = args.Info.SendingPlayer;
            LobbyPlayer lobbyPlayer;
            if (m_LobbyPlayers.TryGetValue(np, out lobbyPlayer))
                lobbyPlayer.UpdateReadyStatus(args.GetNext<bool>());
        });
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

        SetupPlayer(np, args.GetNext<string>(), args.GetNext<ulong>());
    }

    public override void SetPlayerCustomization(RpcArgs args)
    {
        NetworkingPlayer np = args.Info.SendingPlayer;
        m_LobbyPlayers[np].SetCustomization(args.GetNext<ulong>());
    }

    ////////////////////
    ///
    /// Singleton Pattern
    ///
    ////////////////////

    public static LobbySystem Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}
