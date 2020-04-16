using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

/*
 * Persistent Aether network singleton that handles player connections and scene loads.
 */
public class AetherNetworkManager : AetherBehavior
{
    public const int MAX_PLAYER_COUNT = 4;

    // Events for networking interaction
    public event System.Action<Dictionary<NetworkingPlayer, PlayerDetails>> SceneLoaded;

    private Dictionary<NetworkingPlayer, PlayerDetails> m_PlayerDetails;

    // Scene loading
    private HashSet<NetworkingPlayer> m_PlayersLoadedNextScene;

    // Singleton instance
    private static AetherNetworkManager m_Instance;

    private void Awake()
    {
        // Ensure only one of this class exists, and persistent
        if (m_Instance != null)
            Destroy(m_Instance);
        m_Instance = this;
        DontDestroyOnLoad(gameObject);

        m_PlayerDetails = new Dictionary<NetworkingPlayer, PlayerDetails>();
        m_PlayersLoadedNextScene = new HashSet<NetworkingPlayer>();
    }

    private void Start()
    {
        if (NetworkManager.Instance == null)
            return;

        if (NetworkManager.Instance.IsServer)
            NetworkManager.Instance.Networker.playerDisconnected += OnPlayerDisconnected;
    }

    public bool AddPlayer(NetworkingPlayer player, PlayerDetails details)
    {
        if (m_PlayerDetails.ContainsKey(player))
            return false;

        m_PlayerDetails.Add(player, details);
        return true;
    }

    public void LoadScene(SceneIndex sceneIndex)
    {
        Debug.Log("Loading");
        m_PlayersLoadedNextScene.Clear();
        // To trigger events on host when clients finish loading scene
        NetworkManager.Instance.playerLoadedScene += (np, sender) => CheckAllLoadedScene(np);
        StartCoroutine(LoadNextScene(sceneIndex));
    }

    private IEnumerator LoadNextScene(SceneIndex sceneIndex)
    {
        while (networkObject == null)
        {
            Debug.Log("Network not ready");
            yield return null;
        }

        FadeOut();
        // Load loading scene
        yield return SceneManager.LoadSceneAsync((int)SceneIndex.LOADING_SCENE_INDEX);
        // Artificial load time injected to not flicker in/out of loading scene.
        yield return new WaitForSeconds(1.0f);

        FadeOut();
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)sceneIndex);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            if (asyncOp.progress >= 0.9f)
                asyncOp.allowSceneActivation = true;

            yield return null;
        }

        // Scene loaded for host
        NetworkingPlayer host = NetworkManager.Instance.Networker.Me;
        CheckAllLoadedScene(host);
    }

    private void CheckAllLoadedScene(NetworkingPlayer np)
    {
        m_PlayersLoadedNextScene.Add(np);
        if (!m_PlayersLoadedNextScene.IsSupersetOf(m_PlayerDetails.Keys))
            return;

        SceneLoaded?.Invoke(m_PlayerDetails);
    }

    private void OnPlayerDisconnected(NetworkingPlayer np, NetWorker sender)
    {
        if (m_PlayerDetails.ContainsKey(np))
            m_PlayerDetails.Remove(np);
    }

    private void FadeOut()
    {
        networkObject?.SendRpc(RPC_TRIGGER_FADE_OUT, Receivers.All);
    }

    private void FadeIn()
    {
        networkObject?.SendRpc(RPC_TRIGGER_FADE_IN, Receivers.All);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null && NetworkManager.Instance.IsServer)
            NetworkManager.Instance.Networker.playerDisconnected -= OnPlayerDisconnected;
    }

    ////////////////////
    ///
    /// Network RPCs
    ///
    ////////////////////

    // RPC sent by host to trigger Fade out
    public override void TriggerFadeOut(RpcArgs args)
    {
        UXManager.Instance.StartFade();
    }

    // RPC sent by host to trigger Fade in
    public override void TriggerFadeIn(RpcArgs args)
    {
        //TODO
    }

    ////////////////////
    ///
    /// Singleton Pattern
    ///
    ////////////////////

    public static AetherNetworkManager Instance
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
