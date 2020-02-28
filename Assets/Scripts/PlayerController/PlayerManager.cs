using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public System.Action PlayersLoaded;

    private List<Player> m_Players;

    private Player m_LocalPlayer;

    private int m_TotalPlayerCount;

    private void Awake()
    {
        m_Players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        m_Players.Add(player);

        if (m_Players.Count == m_TotalPlayerCount)
        {
            PlayersLoaded();
        }
    }

    public void SetLocalPlayer(Player player)
    {
        m_LocalPlayer = player;
    }

    public void SetPlayerCount(int count)
    {
        m_TotalPlayerCount = count;
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
        return m_Players.FindAll(p => p.GetPlayerDetails().GetTeam() == teamId);
    }
}
