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
    public GameObject restartPanel;
    public Vector2 regionItemsSpawn;
    public List<GameObject> playersInTeamRed, playersInTeamBlue, itemsToBeSpawned;
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
    
    public void InitPlayers(List<GameObject> playersInTeamRed, List<GameObject> playersInTeamBlue)
    {
        this.playersInTeamRed = playersInTeamRed;
        this.playersInTeamBlue = playersInTeamBlue;
    }

    private void Start()
    {
        playersInTeamRed.Add(GameObject.FindGameObjectWithTag("Player"));
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
        restartPanel.SetActive(true);
        enabled = false;
        StartCoroutine("StopRestart");
    }

    public void GameOver(int index)
    {
        //losePanel.SetActive(true);
        enabled = false;
        string loseText = null;
    
        StartCoroutine("StopRestart");
    }

    IEnumerator StopRestart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        restartPanel.SetActive(false);
        // Time.timeScale = 1;
        enabled = true;
    }
}
