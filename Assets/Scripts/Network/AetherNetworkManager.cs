using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class AetherNetworkManager : Singleton<AetherNetworkManager>
{
    public const int MAX_PLAYER_COUNT = 4;

    private const int m_LoadingSceneIndex = 2;

    // Event for networking interaction
    public event System.Action<Dictionary<NetworkingPlayer, PlayerDetails>> SceneLoaded;

    private Dictionary<NetworkingPlayer, PlayerDetails> m_PlayerDetails;

    // Scene loading
    private int m_PlayersLoadedScene;

    void Awake()
    {
        m_PlayerDetails = new Dictionary<NetworkingPlayer, PlayerDetails>();
    }

    void Start()
    {
        // Event triggers on host when clients finish loading scene
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.playerLoadedScene += OnPlayerLoadGameScene;
    }

    public bool AddPlayer(NetworkingPlayer player, PlayerDetails details)
    {
        if (m_PlayerDetails.ContainsKey(player))
            return false;

        m_PlayerDetails.Add(player, details);
        return true;
    }

    public void LoadGame(int sceneId)
    {
        LoadLoadingScene();
        StartCoroutine(LoadGameScene(sceneId));
    }

    private void LoadLoadingScene()
    {
        SceneManager.LoadScene(m_LoadingSceneIndex);
    }

    private IEnumerator LoadGameScene(int sceneId)
    {
        yield return new WaitForSeconds(1.0f);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneId);
        asyncOp.allowSceneActivation = false;

        // Triggered by host when own scene is loaded
        asyncOp.completed += op =>
        {
            NetWorker host = NetworkManager.Instance.Networker;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneId));
            OnPlayerLoadGameScene(host.Me, host);
        };

        while (asyncOp.progress != 0.9f)
            yield return null;

        FadeOut();
        asyncOp.allowSceneActivation = true;
    }

    private void OnPlayerLoadGameScene(NetworkingPlayer np, NetWorker sender)
    {
        m_PlayersLoadedScene++;
        if (m_PlayersLoadedScene == m_PlayerDetails.Count)
        {
            m_PlayersLoadedScene = 0;
            SceneLoaded?.Invoke(m_PlayerDetails);
        }
    }

    private void FadeOut()
    {
        UXManager.Instance.StartFade();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.playerLoadedScene -= OnPlayerLoadGameScene;
    }
}
