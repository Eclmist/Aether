using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event System.Action<GameMode> GameStarted;

    [SerializeField]
    private TowerBase[] m_TowerCheckpoints;

    private float m_GameOverMessageDuration = 10f;

    private bool m_GameStarted = false;

    private Transform m_RespawnPoint;

    private float m_RespawnHeight = -50f;

    private void Awake()
    {
        PlayerManager.Instance.GetPlayerNetworkManager().AllPlayersReady += StartGame;

        if (m_TowerCheckpoints != null)
        {
            foreach (TowerBase tower in m_TowerCheckpoints)
                tower.TowerCaptured += EnableCheckpoint;
        }
    }

    private void Update()
    {
        if (!m_GameStarted)
            return;

        Player player = PlayerManager.Instance.GetLocalPlayer();

        if (player.transform.position.y < m_RespawnHeight)
            player.TriggerRespawn();
    }

    public void StartGame()
    {
        PlayerNetworkManager pnm = PlayerManager.Instance.GetPlayerNetworkManager();
        PlayerDetails details = PlayerManager.Instance.GetLocalPlayer().GetPlayerDetails();
        pnm.AllPlayersReady -= StartGame;
        m_RespawnPoint = pnm.GetSpawnPosition(details.GetPosition());

        Debug.Log("Game started");
        m_GameStarted = true;
        GameStarted?.Invoke(GameMode.GAMEMODE_ZOOM_RACING_CIRCUIT_BREAKER);
    }

    public void GameOver(Player winner)
    {
        StartCoroutine(SetGameOver(winner));
    }

    private IEnumerator SetGameOver(Player winner)
    {
        // Calls UIManager
        // Provides game stats to UIManager to show game over stats
        // for maybe 10 seconds (can be skipped if host presses a button?)
        // then return to lobby
        Debug.Log("Player-" + winner.GetPlayerDetails().GetNetworkId() + " wins");
        UIManager.Instance.ShowWinningMessage(winner);
        yield return new WaitForSeconds(m_GameOverMessageDuration);
        AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.LOBBY_SCENE_INDEX);
    }

    public bool GetGameStarted()
    {
        return m_GameStarted;
    }

    public void EnableCheckpoint(TowerBase tower)
    {
        Debug.Log("Checkpoint tower captured");
        tower.TowerCaptured -= EnableCheckpoint;

        // Replace tower with checkpoint
        Destroy(tower.GetComponent<TowerLocal>());
        tower.GetComponent<Checkpoint>().Activate();
    }

    public Transform GetRespawnPoint()
    {
        return m_RespawnPoint;
    }

    public void SetRespawnPoint(Transform position)
    {
        m_RespawnPoint = position;
    }

    public void RequestGameOver()
    {
        PlayerManager.Instance.GetPlayerNetworkManager()?.RequestGameOver();
    }

    public void SetUnfrozen()
    {
        Player player = PlayerManager.Instance.GetLocalPlayer();
        player.GetComponent<PlayerMovement>().SetFrozen(false);
    }
}
