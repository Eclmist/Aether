using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It's a singleton so call this using GameManager.Instance.
public class GameManager : Singleton<GameManager>
{
    public static int m_WinningScore = 3;

    public List<GameObject> m_TerrainWhereItemsSpawn;
    private List<GameObject> m_RedTeamPlayers;
    private List<GameObject> m_BlueTeamPlayers;
    public List<GameObject> m_SpawnedItems;

    private int m_RedTeamScore, m_BlueTeamScore;
    public int goalsToWin = 3;
    public float itemSpawnDelay = 30;
    
    public void InitPlayers(List<GameObject> playersInTeamRed, List<GameObject> playersInTeamBlue)
    {
        this.m_RedTeamPlayers = playersInTeamRed;
        this.m_BlueTeamPlayers = playersInTeamBlue;
    }
    
    private IEnumerator SpawnItems()
    {
        if (m_SpawnedItems == null || m_TerrainWhereItemsSpawn == null)
        {
            yield break;
        }
        //Handle spawning items here
        GameObject item = m_SpawnedItems[Random.Range(0, m_SpawnedItems.Count)];
        Vector3 spawnPos = m_TerrainWhereItemsSpawn[Random.Range(0, m_TerrainWhereItemsSpawn.Count)].transform.position;
        //itemsToBeSpawned
        Instantiate(item, spawnPos, item.transform.rotation);
        yield return new WaitForSeconds(itemSpawnDelay);
        StartCoroutine(SpawnItems());
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
    
    public void IncrementScore(bool isTeamRed)
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

    public void Win(bool isTeamRed)
    {
        //restartPanel.SetActive(true);
        //enabled = false;
        //StartCoroutine("StopRestart");
    }

    /*public void GameOver(int index)
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
    }*/
}



