using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class LobbyPlayer : LobbyPlayerBehavior
{
    [SerializeField]
    private Text m_PlayerName;

    [SerializeField]
    private Text m_PlayerTeam;

    private int m_Team;

    public string GetName()
    {
        return m_PlayerName.text;
    }

    public int GetTeam()
    {
        return m_Team;
    }

    public void UpdateName(string name)
    {
        m_PlayerName.text = name;

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_NAME, Receivers.All, name);
    }

    public void UpdateTeam(int team)
    {
        m_Team = team;
        if (m_PlayerTeam != null)
        {
            if (m_Team == 0)
            {
                m_PlayerTeam.text = "Red";
            }
            else
            {
                m_PlayerTeam.text = "Blue";
            }
        }

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_TEAM, Receivers.All, m_Team);
    }

    public void UpdateDataFor(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_SET_NAME, m_PlayerName.text);
        networkObject.SendRpc(player, RPC_SET_TEAM, m_Team);
    }

    public override void SetName(RpcArgs args)
    {
        m_PlayerName.text = args.GetNext<string>();
    }

    public override void SetTeam(RpcArgs args)
    {
        m_Team = args.GetNext<int>();
        // E3: No text, this is breaking build
        //if (Team == 0)
        //{
        //    m_PlayerTeam.text = "Red";
        //}
        //else
        //{
        //    m_PlayerTeam.text = "Blue";
        //}
    }
}
