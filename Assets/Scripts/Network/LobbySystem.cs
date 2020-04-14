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

    [SerializeField]
    private GameObject m_StartButton;

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
        {
            NetworkManager.Instance.InstantiateAether();
            NetworkManager.Instance.Networker.playerDisconnected += OnPlayerDisconnected;
        }
        else
        {
            NetworkManager.Instance.Networker.disconnected += OnDisconnected;
        }
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        // This client is accepted and has joined the lobby. (Includes host itself)
        string name = PlayerPrefs.GetString("nickname", "Guest");
        ulong customization = m_CharacterCustomizerLocal.GetDataPacked();
        networkObject.SendRpc(RPC_SET_PLAYER_ENTERED, Receivers.Server, name, customization);

        if (!networkObject.IsServer)
            m_StartButton.SetActive(false);
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

    private void Update()
    {
        if (m_LobbyPlayers.Count == 0)
            return;

        // Handle disconnections
        List<KeyValuePair<NetworkingPlayer, LobbyPlayer>> toDestroy = new List<KeyValuePair<NetworkingPlayer, LobbyPlayer>>();

        foreach (var entry in m_LobbyPlayers)
        {
            if (entry.Value.GetIsDisconnected())
            {
                toDestroy.Add(entry);

                m_Loaders[entry.Value.GetPosition()].SetActive(true);
            }
        }

        if (toDestroy.Count > 0)
        {
            foreach (var entry in toDestroy)
            {
                m_LobbyPlayers.Remove(entry.Key);
                Destroy(entry.Value);
            }
        }

        // Handle reslotting
        int currentIndex = 0;
        List<LobbyPlayer> players = new List<LobbyPlayer>(m_LobbyPlayers.Values);
        players.Sort((a, b) => a.GetPosition() - b.GetPosition());
        foreach (LobbyPlayer player in players)
        {
            int position = player.GetPosition();
            if (position >= m_PlayerPositions.Length)
                continue;

            if (position > currentIndex)
            {
                player.UpdatePosition(currentIndex);
            }
            currentIndex++;
        }
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

        AetherNetworkManager.Instance.LoadScene(SceneIndex.FUNRUN_SCENE_INDEX);
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

    private void OnPlayerDisconnected(NetworkingPlayer np, NetWorker sender)
    {
        LobbyPlayer lobbyPlayer;
        m_LobbyPlayers.TryGetValue(np, out lobbyPlayer);
        if (lobbyPlayer != null)
            lobbyPlayer.Disconnect();
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

    // RPC sent to host by player finalizing customization
    public override void SetPlayerCustomization(RpcArgs args)
    {
        NetworkingPlayer np = args.Info.SendingPlayer;
        m_LobbyPlayers[np].SetCustomization(args.GetNext<ulong>());
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

    private void OnDisconnected(NetWorker sender)
    {
        AetherNetworkManager.Instance.LoadScene(SceneIndex.TITLE_SCENE_INDEX);
    }

    public void SetPlayerInPosition(LobbyPlayer player)
    {
        int index = player.GetPosition();
        if (index >= m_PlayerPositions.Length)
            return;

        Destroy(m_Loaders[index]);
        Transform parent = m_PlayerPositions[index];
        player.transform.SetParent(parent, false);
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
