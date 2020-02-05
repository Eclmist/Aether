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

    public string Name
    {
        get
        {
            return m_PlayerName.text;
        }
    }

    public int Team { get; private set; }

    public void UpdateName(string name)
    {
        m_PlayerName.text = name;

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_NAME, Receivers.All, name);
    }

    public void UpdateTeam(int team)
    {
        Team = team;
        if (Team == 0)
        {
            m_PlayerTeam.text = "Red";
        }
        else
        {
            m_PlayerTeam.text = "Blue";
        }

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_TEAM, Receivers.All, Team);
    }

    public void UpdateDataFor(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_SET_NAME, m_PlayerName.text);
        networkObject.SendRpc(player, RPC_SET_TEAM, Team);
    }

    public override void SetName(RpcArgs args)
    {
        m_PlayerName.text = args.GetNext<string>();
    }

    public override void SetTeam(RpcArgs args)
    {
        Team = args.GetNext<int>();
        if (Team == 0)
        {
            m_PlayerTeam.text = "Red";
        }
        else
        {
            m_PlayerTeam.text = "Blue";
        }
    }
}
