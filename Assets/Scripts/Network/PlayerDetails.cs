public class PlayerDetails
{
    private uint m_NetworkId;
    private Team m_Team;
    private int m_Position;
    private ulong m_Customization;

    public PlayerDetails(uint networkId, Team team, int position, ulong customization)
    {
        m_NetworkId = networkId;
        m_Team = team;
        m_Position = position;
        m_Customization = customization;
    }

    public void SetNetworkId(uint networkId)
    {
        m_NetworkId = networkId;
    }

    public void SetTeam(Team team)
    {
        m_Team = team;
    }

    public void SetPosition(int position)
    {
        m_Position = position;
    }

    public void SetCustomization(ulong customization)
    {
        m_Customization = customization;
    }

    public uint GetNetworkId()
    {
        return m_NetworkId;
    }

    public Team GetTeam()
    {
        return m_Team;
    }

    public int GetPosition()
    {
        return m_Position;
    }

    public ulong GetCustomization()
    {
        return m_Customization;
    }

    public object[] ToArray()
    {
        return new object[] { m_NetworkId, (int)m_Team, m_Position, m_Customization };
    }

    public static PlayerDetails FromArray(object[] arr)
    {
        if (arr.Length != 4)
            return null;

        return new PlayerDetails((uint)arr[0], (Team)arr[1], (int)arr[2], (ulong)arr[3]);
    }
}