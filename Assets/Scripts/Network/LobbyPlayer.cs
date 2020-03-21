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

    [SerializeField]
    private Image m_ReadyStatus;

    private Team m_Team;

    private bool m_IsReady;

    public void ToggleReady()
    {
        m_IsReady = !(m_IsReady);
        IndicateReady();
    }

    private void IndicateReady()
    {
        if (m_ReadyStatus == null)
            return;

        var originalColor = m_ReadyStatus.color;

        if (m_IsReady)
            m_ReadyStatus.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);
        else 
            m_ReadyStatus.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);

    }

    public string GetName()
    {
        return m_PlayerName.text;
    }

    public Team GetTeam()
    {
        return m_Team;
    }

    public void UpdateName(string name)
    {
        m_PlayerName.text = name;

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_NAME, Receivers.All, name);
    }

    public void UpdateTeam(Team team)
    {
        m_Team = team;
        if (m_PlayerTeam != null)
        {
            switch (m_Team)
            {
                case Team.TEAM_ONE:
                    m_PlayerTeam.text = "Red";
                    break;
                case Team.TEAM_TWO:
                    m_PlayerTeam.text = "Blue";
                    break;
                default:
                    break;
            }
        }

        if (networkObject != null)
            networkObject.SendRpc(RPC_SET_TEAM, Receivers.All, (int)m_Team);
    }

    public void UpdateDataFor(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_SET_NAME, m_PlayerName.text);
        networkObject.SendRpc(player, RPC_SET_TEAM, (int)m_Team);
    }

    public override void SetName(RpcArgs args)
    {
        m_PlayerName.text = args.GetNext<string>();
    }

    public override void SetTeam(RpcArgs args)
    {
        m_Team = (Team)args.GetNext<int>();
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
