using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{
    public static int m_WinningScore = 3;

    private List<GameObject> m_RedTeamPlayers;
    private List<GameObject> m_BlueTeamPlayers;

    private int m_RedTeamScore, m_BlueTeamScore;

    public Int32 GoalsScoredRed
    {
        get => m_RedTeamScore;
        set => m_RedTeamScore = value;
    }

    public Int32 GoalsScoredBlue
    {
        get => m_BlueTeamScore;
        set => m_BlueTeamScore = value;
    }
    
    public void InitPlayers(List<GameObject> playersInTeamRed, List<GameObject> playersInTeamBlue)
    {
        this.m_RedTeamPlayers = playersInTeamRed;
        this.m_BlueTeamPlayers = playersInTeamBlue;
    }

    private void Start()
    {
        //playersInTeamRed.Add(GameObject.FindGameObjectWithTag("Player"));
        //StartCoroutine(SpawnItems()); // E3: SpawnItems is throwing errors array out of bound
    }

    public void IncrementScore(GameObject playerWhoScored)
    {
        if(m_RedTeamPlayers.Contains(playerWhoScored))
        {
            m_RedTeamScore++;
        }
        else
        {
            m_BlueTeamScore++;
        }

        CheckWin();
    }
    
    public void IncrementScore(Boolean isTeamRed)
    {
        if (isTeamRed)
        {
            m_RedTeamScore++;
        }
        else
        {
            m_BlueTeamScore++;
        }

        CheckWin();
    }

    private void CheckWin()
    {
        if (m_RedTeamScore >= m_WinningScore)
        {
            Win(true);
        } else if (m_BlueTeamScore >= m_WinningScore)
        {
            Win(false);
        }
    }

    public void Win(Boolean isTeamRed)
    {
        //restartPanel.SetActive(true);
        enabled = false;
        StartCoroutine("StopRestart");
    }

    public void GameOver(int index)
    {
    }
}
