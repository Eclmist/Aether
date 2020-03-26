using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

/*
 * Manager that is only used by host to manage connections and scene changes
 */
public class AetherNetworkManager : Singleton<AetherNetworkManager>
{
    public const int MAX_PLAYER_COUNT = 4;

    public const int LOBBY_SCENE_INDEX = 1;
    public const int LOADING_SCENE_INDEX = 2;
    public const int KOTH_SCENE_INDEX = 3;

    // Event for pre-scene switch clean up
    public event System.Action CleanUpCurrentScene;

    // Events for networking interaction
    public event System.Action<NetworkingPlayer> PlayerDisconnected;
    public event System.Action<Dictionary<NetworkingPlayer, PlayerDetails>> SceneLoaded;

    private Dictionary<NetworkingPlayer, PlayerDetails> m_PlayerDetails;

    // Scene loading
    private HashSet<NetworkingPlayer> m_PlayersLoadedNextScene;

    void Awake()
    {
        m_PlayerDetails = new Dictionary<NetworkingPlayer, PlayerDetails>();
        m_PlayersLoadedNextScene = new HashSet<NetworkingPlayer>();
    }

    void Start()
    {
        if (NetworkManager.Instance != null)
        {
            // Event triggers on host when clients finish loading scene
            NetworkManager.Instance.playerLoadedScene += (np, sender) =>
            {
                m_PlayersLoadedNextScene.Add(np);
            };

            NetworkManager.Instance.Networker.playerDisconnected += OnPlayerDisconnect;
        }
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
        CleanUpCurrentScene?.Invoke();
        LoadLoadingScene();
        StartCoroutine(LoadNextScene(sceneId));
    }

    private void LoadLoadingScene()
    {
        SceneManager.LoadScene(LOADING_SCENE_INDEX);
    }

    public IEnumerator LoadNextScene(int sceneId)
    {
        yield return new WaitForSeconds(2.0f);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneId);
        asyncOp.allowSceneActivation = false;

        // Triggered by host when own scene is loaded
        asyncOp.completed += op =>
        {
            NetworkingPlayer host = NetworkManager.Instance.Networker.Me;
            m_PlayersLoadedNextScene.Add(host);
            StartCoroutine(CheckAllLoadedScene());
        };

        while (asyncOp.progress < 0.9f)
            yield return null;

        FadeOut();
        asyncOp.allowSceneActivation = true;
    }

    private IEnumerator CheckAllLoadedScene()
    {
        while (!m_PlayersLoadedNextScene.IsSupersetOf(m_PlayerDetails.Keys))
            yield return null;

        m_PlayersLoadedNextScene.Clear();
        SceneLoaded?.Invoke(m_PlayerDetails);
    }

    private void OnPlayerDisconnect(NetworkingPlayer np, NetWorker sender)
    {
        if (m_PlayerDetails.ContainsKey(np))
        {
            m_PlayerDetails.Remove(np);
            PlayerDisconnected?.Invoke(np);
        }
    }

    private void FadeOut()
    {
        UXManager.Instance.StartFade();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.Networker.playerDisconnected -= OnPlayerDisconnect;
        }
    }
}
