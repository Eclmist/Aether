using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Tower[] m_Towers;

    [SerializeField]
    private PlayerNetworkManager m_PlayerNetworkManager;

    private int m_Team1CaptureCount = 0;
    private int m_Team2CaptureCount = 0;

    private bool m_GameStarted = false;

    private void Awake()
    {
        m_PlayerNetworkManager.PlayersReady += StartGame;
    }

    private void Update()
    {
        if (!m_GameStarted)
            return;

        foreach (Tower tower in m_Towers)
        {
            Tower.CaptureState captureState = tower.GetCaptureState();

            if (captureState.IsCaptured())
            {
                if (captureState.GetTeam() == 0)
                    m_Team1CaptureCount++;
                else
                    m_Team2CaptureCount++;
            }
        }

        if (m_Team1CaptureCount == m_Towers.Length)
            SetGameOver(0);
        else if (m_Team2CaptureCount == m_Towers.Length)
            SetGameOver(1);
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
