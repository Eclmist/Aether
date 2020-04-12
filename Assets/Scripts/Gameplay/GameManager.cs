using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event System.Action<GameMode> GameStarted;

    private float m_GameOverMessageDuration = 10f;

    private bool m_GameStarted = false;

    private Transform m_RespawnPoint;

    private void Awake()
    {
        PlayerManager.Instance.GetPlayerNetworkManager().AllPlayersReady += StartGame;
    }

    public void StartGame()
    {
        PlayerManager.Instance.GetPlayerNetworkManager().AllPlayersReady -= StartGame;
        Debug.Log("Game started");
        m_GameStarted = true;
        GameStarted?.Invoke(GameMode.GAMEMODE_ZOOM_RACING_CIRCUIT_BREAKER);
    }

    public void GameOver(Player winner)
    {
        StartCoroutine(SetGameOver(winner));
    }

    private IEnumerator SetGameOver(Player winner)
    {
        // Calls UIManager
        // Provides game stats to UIManager to show game over stats
        // for maybe 10 seconds (can be skipped if host presses a button?)
        // then return to lobby
        UIManager.Instance.ShowWinningMessage(winner);
        yield return new WaitForSeconds(m_GameOverMessageDuration);
        AetherNetworkManager.Instance.LoadScene(AetherNetworkManager.LOBBY_SCENE_INDEX);
    }

    public bool GetGameStarted()
    {
        return m_GameStarted;
    }

    public Transform GetRespawnPoint()
    {
        return m_RespawnPoint;
    }

    public void SetRespawnPoint(Transform position)
    {
        m_RespawnPoint = position;
    }

    public void RequestGameOver()
    {
        PlayerManager.Instance.GetPlayerNetworkManager()?.RequestGameOver();
    }

    public void SetUnfrozen()
    {
        Player player = PlayerManager.Instance.GetLocalPlayer();
        player.GetComponent<PlayerMovement>().SetFrozen(false);
    }
}
