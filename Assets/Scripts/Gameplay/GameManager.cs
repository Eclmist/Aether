using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event System.Action<GameMode> GameStarted;

    [SerializeField]
    private TowerBase[] m_TowerCheckpoints;

    [SerializeField]
    private Endpoint m_Endpoint;

    private float m_GameOverMessageDuration = 10f;

    private bool m_GameStarted = false;

    // Checkpoint Mechanic
    private Transform m_RespawnPoint;
    private float m_RespawnHeight = -50f;
    private int m_CurrentCheckpointPriority = 0;

    // Current Game Progress;
    private float m_StartZ = 0.0f;
    private float m_EndZ = 0.0f;
    private Dictionary<Player, float> m_CurrentProgress;
    private TowerBase m_CurrentTower;

    private void Awake()
    {
        PlayerManager.Instance.GetPlayerNetworkManager().AllPlayersReady += StartGame;

        if (m_TowerCheckpoints != null)
        {
            foreach (TowerBase tower in m_TowerCheckpoints)
            {
                tower.TowerCaptured += TowerCheckpointCaptured;
                tower.TowerEntered += SetCurrentTower;
                tower.TowerExited += () => SetCurrentTower(null);
            }
        }

        m_CurrentProgress = new Dictionary<Player, float>();
    }

    private void Update()
    {
        if (!m_GameStarted)
            return;

        Player lp = PlayerManager.Instance.GetLocalPlayer();

        if (lp.transform.position.y < m_RespawnHeight)
            lp.TriggerRespawn();

        if (m_CurrentProgress.Count == 0)
            return;

        foreach (Player player in PlayerManager.Instance.GetAllPlayers())
        {
            if (!m_CurrentProgress.ContainsKey(player))
                continue;

            float currentZ = player.transform.position.z;
            m_CurrentProgress[player] = Mathf.Clamp01((currentZ - m_StartZ) / (m_EndZ - m_StartZ));
        }
    }

    public void StartGame()
    {
        PlayerNetworkManager pnm = PlayerManager.Instance.GetPlayerNetworkManager();
        PlayerDetails details = PlayerManager.Instance.GetLocalPlayer().GetPlayerDetails();
        pnm.AllPlayersReady -= StartGame;
        m_RespawnPoint = pnm.GetSpawnPosition(details.GetPosition());

        m_StartZ = m_RespawnPoint.position.z;
        m_EndZ = m_Endpoint.transform.position.z;

        foreach (Player player in PlayerManager.Instance.GetAllPlayers())
        {
            m_CurrentProgress.Add(player, 0.0f);
        }

        Debug.Log("Game started");
        m_GameStarted = true;
        GameStarted?.Invoke(GameMode.GAMEMODE_ZOOM_RACING_CIRCUIT_BREAKER);
        m_TowerCheckpoints[0].Activate();
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
        string winnerName = winner.GetPlayerDetails().GetName();
        Debug.Log(winnerName + " wins");
        //UIManager.Instance.UINotifySecondary(winnerName + " has reached the finish line.");
        UIManager.Instance.ShowWinningMessage(winner);
        yield return new WaitForSeconds(m_GameOverMessageDuration);
        AetherNetworkManager.Instance.LoadScene(SceneIndex.LOBBY_SCENE_INDEX);
    }

    public bool GetGameStarted()
    {
        return m_GameStarted;
    }

    public float GetPlayerProgress(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= PlayerManager.Instance.GetAllPlayers().Count)
            return 0;

        Player player = PlayerManager.Instance.GetAllPlayers()[playerIndex];
        float progress = -1;
        m_CurrentProgress.TryGetValue(player, out progress);
        return progress;
    }

    public Transform GetRespawnPoint()
    {
        return m_RespawnPoint;
    }

    public int GetCheckpointPriority()
    {
        return m_CurrentCheckpointPriority;
    }

    public TowerBase GetCurrentTower()
    {
        return m_CurrentTower;
    }

    public void SetRespawnPoint(Transform position)
    {
        m_RespawnPoint = position;
    }

    public void SetCurrentTower(TowerBase tower)
    {
        m_CurrentTower = tower;
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

    public void TowerCheckpointCaptured(TowerBase tower)
    {
        UIManager.Instance.UINotifyHeader("Checkpoint Captured!");
        UIManager.Instance.UINotifySecondary("The next checkpoint has been revealed.");
        Debug.Log("Checkpoint tower captured");
        tower.TowerCaptured -= TowerCheckpointCaptured;
        // Prevent notification from calling again
        tower.TowerEntered = null;
        tower.TowerExited = null;
        SetCurrentTower(null);
        tower.RevealNext();

        // Replace tower with checkpoint
        TowerHost th = tower.GetComponent<TowerHost>();
        TowerClient tc = tower.GetComponent<TowerClient>();
        if (th != null)
            Destroy(th);
        if (tc != null)
            Destroy(tc);

        int priority = tower.GetPriority();
        if (m_CurrentCheckpointPriority <= priority)
        {
            tower.GetComponent<Checkpoint>().Activate();
            m_CurrentCheckpointPriority = priority;
        }
    }
}
