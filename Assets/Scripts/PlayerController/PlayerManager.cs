using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public System.Action PlayersLoaded;

    private List<Player> m_Players;

    private List<int> m_Team1Indices;
    private List<int> m_Team2Indices;

    private Player m_LocalPlayer;

    private int m_TotalPlayerCount;

    private void Awake()
    {
        m_Players = new List<Player>();
        m_Team1Indices = new List<int>();
        m_Team2Indices = new List<int>();
    }

    public void AddPlayer(Player player)
    {
        m_Players.Add(player);

        if (IsFromTeam(player, 0))
            m_Team1Indices.Add(m_Players.Count - 1);
        else
            m_Team2Indices.Add(m_Players.Count - 1);

        if (m_Players.Count == m_TotalPlayerCount)
            PlayersLoaded();
    }

    public bool IsFromTeam(Player player, int teamId)
    {
        PlayerDetails playerDetails = player.GetPlayerDetails();
        if (playerDetails == null)
            return false;

        return playerDetails.GetTeam() == teamId;
    }

    public void SetLocalPlayer(Player player)
    {
        m_LocalPlayer = player;
    }

    public void SetPlayerCount(int count)
    {
        m_TotalPlayerCount = count;
    }

    public int GetPlayerCount()
    {
        return m_TotalPlayerCount;
    }

    public Player GetLocalPlayer()
    {
        return m_LocalPlayer;
    }

    public List<Player> GetAllPlayers()
    {
        return m_Players;
    }

    public List<Player> GetPlayersByTeam(int teamId)
    {
        if (teamId == 0)
            return m_Team1Indices.ConvertAll(index => m_Players[index]);
        else
            return m_Team2Indices.ConvertAll(index => m_Players[index]);
    }
}
