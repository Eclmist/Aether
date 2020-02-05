using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbySystem : LobbySystemBehavior
{
    [SerializeField]
    private List<Transform> m_PlayerPositions;

    [SerializeField]
    private List<GameObject> m_Loaders;

    private List<LobbyPlayer> m_LobbyPlayers;

    private int m_PlayerCount;

    void Awake()
    {
        m_LobbyPlayers = new List<LobbyPlayer>();
        m_PlayerCount = 0;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        if (!networkObject.IsServer)
            return;

        // Setup host
        SetupPlayer(networkObject.Networker.Me.NetworkId);

        NetworkManager.Instance.Networker.playerAccepted += OnPlayerAccepted;
    }

    private void SetupPlayer(uint playerId)
    {
        MainThreadManager.Run(() => {
            Vector3 position = m_PlayerPositions[m_PlayerCount].position;
            Quaternion rotation = m_PlayerPositions[m_PlayerCount].rotation;
            Destroy(m_Loaders[m_PlayerCount]);

            LobbyPlayer player = (LobbyPlayer)NetworkManager.Instance.InstantiateLobbyPlayer(rotation : rotation);
            player.transform.SetParent(m_PlayerPositions[m_PlayerCount].transform, false);
            m_LobbyPlayers.Add(player);
            // Name setup
            string playerName = "Player-" + playerId;
            player.UpdateName(playerName);
            m_PlayerCount++;
        });
    }

    private IEnumerator StartGame(int sceneID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    private void OnPlayerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        foreach (LobbyPlayer p in m_LobbyPlayers)
            p.UpdateNameFor(player);
   
        SetupPlayer(player.NetworkId);
    }

    public void OnStart()
    {
        if (NetworkManager.Instance.IsServer)
            StartCoroutine(StartGame(2));
    }
}
