using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event System.Action<GameMode> GameStarted;

    [SerializeField]
    private TowerBase[] m_Towers;

    [SerializeField]
    private GameObject m_Managers;

    private Dictionary<Team, int> m_TeamCaptureCounts;

    private int m_TotalCaptureCount = 0;

    private bool m_GameStarted = false;

    private void Awake()
    {
        AetherNetworkManager.Instance.CleanUpCurrentScene += CleanUpScene;
        PlayerManager.Instance.GetPlayerNetworkManager().AllPlayersReady += StartGame;
        m_TeamCaptureCounts = new Dictionary<Team, int>();

        foreach (TowerBase tower in m_Towers)
            tower.TowerCaptured += OnTowerCaptured;

        foreach (Team team in (Team[])System.Enum.GetValues(typeof(Team)))
            m_TeamCaptureCounts.Add(team, 0);
    }

    private void OnTowerCaptured(TowerBase tower)
    {
        if (!m_GameStarted)
            return;

        tower.SetIsCaptured(true);
        m_TotalCaptureCount++;

        TowerBase.CaptureState captureState = tower.GetCaptureState();
        m_TeamCaptureCounts[captureState.GetLeadingTeam()]++;

        // Check for game over
        if (m_TotalCaptureCount == m_Towers.Length)
        {
            Team winningTeam = Team.TEAM_ONE;
            int winningScore = 0;
            foreach (KeyValuePair<Team, int> pair in m_TeamCaptureCounts)
            {
                if (pair.Value > winningScore)
                {
                    winningTeam = pair.Key;
                    winningScore = pair.Value;
                }
            }
            SetGameOver(winningTeam);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game started");
        m_GameStarted = true;
        GameStarted?.Invoke(GameMode.GAMEMODE_KING_OF_THE_HILL);

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(5f);
        AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.LOBBY_SCENE_INDEX);
    }

    public void SetGameOver(Team team)
    {
        Debug.Log("Team " + (int)team + " wins");

        // Calls UIManager
        // Provides game stats to UIManager to show game over stats
        // for maybe 10 seconds (can be skipped if host presses a button?)
        // then return to lobby
    }

    private void CleanUpScene()
    {
        // Destroy all non-persisting singletons (Managers)
        Destroy(m_Managers);
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerNetworkManager playerNetworkManager = PlayerManager.Instance.GetPlayerNetworkManager();
            if (playerNetworkManager != null)
                playerNetworkManager.AllPlayersReady -= StartGame;
        }
    }
}
