using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{
    public GameObject losePanel, winPanel;
    
    public GameObject[] players, itemToBeSpawned;
    private PlayerMovement playerMovement;
    private int goalsScoredRed, goalsScoredBlue;
    public int goalsToWin = 3;

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

    void Start()
    {
        //healthSystem = GetComponent<HealthSystem>();
        //playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        SpawnItems();
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
    


    private void SpawnItems()
    {
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
