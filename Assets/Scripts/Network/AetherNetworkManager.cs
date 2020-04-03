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

    public const int LOBBY_SCENE_INDEX = 1;
    public const int LOADING_SCENE_INDEX = 2;
    public const int KOTH_SCENE_INDEX = 3;

    // Events for networking interaction
    public event System.Action<NetworkingPlayer> PlayerDisconnected;
    public event System.Action<Dictionary<NetworkingPlayer, PlayerDetails>> SceneLoaded;

    private Dictionary<NetworkingPlayer, PlayerDetails> m_PlayerDetails;

    // Scene loading
    private HashSet<NetworkingPlayer> m_PlayersLoadedNextScene;

    // Singleton-pattern
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static AetherNetworkManager m_Instance;

    private void Awake()
    {
        m_PlayerDetails = new Dictionary<NetworkingPlayer, PlayerDetails>();
        m_PlayersLoadedNextScene = new HashSet<NetworkingPlayer>();
    }

    private void Start()
    {
        NetworkManager.Instance.Networker.playerDisconnected += OnPlayerDisconnect;
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
        Debug.Log("Loading");
        m_PlayersLoadedNextScene.Clear();
        // To trigger events on host when clients finish loading scene
        NetworkManager.Instance.playerLoadedScene += (np, sender) => CheckAllLoadedScene(np);
        StartCoroutine(LoadNextScene(sceneId));
    }

    private IEnumerator LoadNextScene(int sceneId)
    {
        FadeOut();
        // Load loading scene
        yield return SceneManager.LoadSceneAsync(LOADING_SCENE_INDEX);
        // Artificial load time injected to not flicker in/out of loading scene.
        yield return new WaitForSeconds(1.0f);

        FadeOut();
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneId);
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
        networkObject.SendRpc(RPC_TRIGGER_FADE_OUT, Receivers.All);
    }

    private void FadeIn()
    {
        networkObject.SendRpc(RPC_TRIGGER_FADE_IN, Receivers.All);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.Networker.playerDisconnected -= OnPlayerDisconnect;
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
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance 'AetherNetworkManager' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = FindObjectOfType<AetherNetworkManager>();

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<AetherNetworkManager>();
                        singletonObject.name = "AetherNetworkManager (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }

    public static bool HasInstance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<AetherNetworkManager>();

            return m_Instance != null;
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
}
