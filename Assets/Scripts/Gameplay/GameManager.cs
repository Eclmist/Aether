using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{
    public GameObject losePanel, winPanel;
    
    public GameObject[] playersInTeamRed, playersInTeamBlue, itemsToBeSpawned;
    private int goalsScoredRed, goalsScoredBlue;
    public int goalsToWin = 3;
    public float itemSpawnDelay = 30;

    public Int32 GoalsScoredRed
    {
        get => goalsScoredRed;
        set => goalsScoredRed = value;
    }

    public Int32 GoalsScoredBlue
    {
        get => goalsScoredBlue;
        set => goalsScoredBlue = value;
    }
    
    public void InitPlayers(GameObject[] playersInTeamRed, GameObject[] playersInTeamBlue)
    {
        this.playersInTeamRed = playersInTeamRed;
        this.playersInTeamBlue = playersInTeamBlue;
    }

    private void Update()
    {
    }

    private IEnumerator SpawnItems()
    {
        //Handle spawning items here
        //itemsToBeSpawned
        yield return new WaitForSeconds(itemSpawnDelay);
        StartCoroutine(SpawnItems());
    }
    
    public void Scored(GameObject playerWhoScored)
    {
        if(playersInTeamRed.Contains(playerWhoScored))
        {
            goalsScoredRed++;
        }
        else
        {
            goalsScoredBlue++;
        }

        CheckWin();
    }
    
    public void Scored(Boolean isTeamRed)
    {
        if (isTeamRed)
        {
            goalsScoredRed++;
        }
        else
        {
            goalsScoredBlue++;
        }

        CheckWin();
    }

    private void CheckWin()
    {
        if (goalsScoredRed >= goalsToWin)
        {
            Win(true);
        } else if (goalsScoredBlue >= goalsToWin)
        {
            Win(false);
        }
    }

    public void Win(Boolean isTeamRed)
    {
        // winPanel.SetActive(true);
        // enabled = false;
        // StartCoroutine("StopCongrats");
    }

    public void GameOver(int index)
    {
        losePanel.SetActive(true);
        enabled = false;
        string loseText = null;
    
        StartCoroutine("StopRestart");
    }

    IEnumerator StopRestart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        losePanel.SetActive(false);
        // Time.timeScale = 1;
        enabled = true;
    }
}
