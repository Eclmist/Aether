using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private TowerBase[] m_Towers;

    [SerializeField]
    private PlayerNetworkManager m_PlayerNetworkManager;

    private Dictionary<Team, int> m_TeamCaptureCounts;

    private int m_TotalCaptureCount = 0;

    private bool m_GameStarted = false;

    private void Awake()
    {
        m_PlayerNetworkManager.PlayersReady += StartGame;
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
    }

    public void SetGameOver(Team team)
    {
        Debug.Log("Team " + (int)team + " wins");
    }
}
