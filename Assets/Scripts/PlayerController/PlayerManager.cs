using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public event System.Action PlayerListPopulated;

    private Player m_LocalPlayer;

    private List<Player> m_Players;
    private Dictionary<Team, List<int>> m_TeamIndices;

    private int m_TotalPlayerCount;

    private void Awake()
    {
        m_Players = new List<Player>();
        m_TeamIndices = new Dictionary<Team, List<int>>();

        foreach (Team team in (Team[])System.Enum.GetValues(typeof(Team)))
            m_TeamIndices.Add(team, new List<int>());
    }

    public void AddPlayer(Player player)
    {
        m_Players.Add(player);

        PlayerDetails details = player.GetPlayerDetails();
        if (details == null)
        {
            Debug.Log("Player details are null. PlayerManager.AddPlayer");
            return;
        }

        // Add index to the appropriate team list
        Team team = details.GetTeam();
        m_TeamIndices[team].Add(m_Players.Count - 1);

        // Check if all players are loaded into the lists
        if (m_Players.Count == m_TotalPlayerCount)
            PlayerListPopulated();
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

    public Player GetPlayerById(uint networkId)
    {
        return m_Players.Find(player => player.GetPlayerDetails().GetNetworkId() == networkId);
    }

    public List<Player> GetAllPlayers()
    {
        return m_Players;
    }

    public List<Player> GetPlayersByTeam(Team team)
    {
        return m_TeamIndices[team].ConvertAll(index => m_Players[index]);
    }

    public List<Player> GetTeamMembers()
    {
        List<Player> teamPlayers = GetPlayersByTeam(m_LocalPlayer.GetPlayerDetails().GetTeam());

        if (!teamPlayers.Remove(m_LocalPlayer))
            Debug.Log("Local player not found in his team's list");

        return teamPlayers;
    }
}
