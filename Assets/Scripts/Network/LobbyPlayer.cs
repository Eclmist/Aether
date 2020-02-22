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

    private int m_TeamId;

    public string GetName()
    {
        return m_PlayerName.text;
    }

    public int GetTeam()
    {
        return m_TeamId;
    }

    public void UpdateName(string name)
    {
        // m_PlayerName.text = name;

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_NAME, Receivers.All, name);
    }

    public void UpdateTeam(int teamId)
    {
        m_TeamId = teamId;
        if (m_PlayerTeam != null)
        {
            if (m_TeamId == 0)
            {
                m_PlayerTeam.text = "Red";
            }
            else
            {
                m_PlayerTeam.text = "Blue";
            }
        }

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_TEAM, Receivers.All, m_TeamId);
    }

    public void UpdateDataFor(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_SET_NAME, m_PlayerName.text);
        networkObject.SendRpc(player, RPC_SET_TEAM, m_TeamId);
    }

    public override void SetName(RpcArgs args)
    {
        m_PlayerName.text = args.GetNext<string>();
    }

    public override void SetTeam(RpcArgs args)
    {
        m_TeamId = args.GetNext<int>();
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
