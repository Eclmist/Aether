using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private TowerBase[] m_Towers;

    [SerializeField]
    private PlayerNetworkManager m_PlayerNetworkManager;

    private int m_Team1CaptureCount = 0;
    private int m_Team2CaptureCount = 0;

    private bool m_GameStarted = false;

    private void Awake()
    {
        m_PlayerNetworkManager.PlayersReady += StartGame;
        foreach (TowerBase tower in m_Towers)
        {
            tower.TowerCaptured += OnTowerCaptured;
        }
    }

    private void OnTowerCaptured(TowerBase tower)
    {
        if (!m_GameStarted)
            return;

        tower.SetIsCaptured(true);

        TowerBase.CaptureState captureState = tower.GetCaptureState();
        if (captureState.GetLeadingTeam() == 0)
            m_Team1CaptureCount++;
        else
            m_Team2CaptureCount++;

        // Check for game over
        if (m_Team1CaptureCount + m_Team2CaptureCount == m_Towers.Length)
            SetGameOver(m_Team1CaptureCount > m_Team2CaptureCount ? 0 : 1);
    }

    public void StartGame()
    {
        Debug.Log("Game started");
        m_GameStarted = true;
    }

    public void SetGameOver(int teamId)
    {
        Debug.Log("Team " + teamId + " wins");
    }
}
