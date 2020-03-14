using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class AetherNetworkManager : Singleton<AetherNetworkManager>
{
    public static int MAX_PLAYER_COUNT = 4;

    // Events for networking interaction
    public System.Action<Dictionary<NetworkingPlayer, PlayerDetails>> SceneChanged;

    private Dictionary<NetworkingPlayer, PlayerDetails> m_PlayerDetails;

    private int m_PlayersLoadedScene;

    void Awake()
    {
        m_PlayerDetails = new Dictionary<NetworkingPlayer, PlayerDetails>();
    }

    void Start()
    {
        // Event triggers on host when clients finish loading scene
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.playerLoadedScene += OnPlayerLoadScene;
    }

    public bool AddPlayer(NetworkingPlayer player, PlayerDetails details)
    {
        if (m_PlayerDetails.ContainsKey(player))
            return false;

        m_PlayerDetails.Add(player, details);
        return true;
    }

    public void LoadScene(int sceneId)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneId);

        loadAsync.completed += asyncOp =>
        {
            NetWorker sender = NetworkManager.Instance.Networker;
            // When host finishes loading scene
            OnPlayerLoadScene(sender.Me, sender);
        };
    }

    public void OnPlayerLoadScene(NetworkingPlayer player, NetWorker sender)
    {
        m_PlayersLoadedScene++;
        if (m_PlayersLoadedScene == m_PlayerDetails.Count)
        {
            m_PlayersLoadedScene = 0;
            SceneChanged(m_PlayerDetails);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.playerLoadedScene -= OnPlayerLoadScene;
    }
}
