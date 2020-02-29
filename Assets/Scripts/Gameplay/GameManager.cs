using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private Tower[] m_Towers;

    private int m_Team1CaptureCount = 0;
    private int m_Team2CaptureCount = 0;

    private void Update()
    {
        foreach (Tower tower in m_Towers)
        {
            // Check tower to get state of capture (range between -100 to 100)
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
    }

    private void GameOverCheck()
    {
    }

    public void SetGameOver(int index)
    {
    }
}
