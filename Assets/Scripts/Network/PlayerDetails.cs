public class PlayerDetails
{
    private uint m_NetworkId;
    private int m_TeamId;
    private int m_Position;

    public PlayerDetails(uint networkId, int teamId, int position)
    {
        m_NetworkId = networkId;
        m_TeamId = teamId;
        m_Position = position;
    }

    public void SetNetworkId(uint networkId)
    {
        m_NetworkId = networkId;
    }

    public void SetTeam(int teamId)
    {
        m_TeamId = teamId;
    }

    public void SetPosition(int position)
    {
        m_Position = position;
    }

    public uint GetNetworkId()
    {
        return m_NetworkId;
    }

    public int GetTeam()
    {
        return m_TeamId;
    }

    public int GetPosition()
    {
        return m_Position;
    }

    public object[] ToArray()
    {
        return new object[] { m_NetworkId, m_TeamId, m_Position };
    }

    public static PlayerDetails FromArray(object[] arr)
    {
        if (arr.Length != 3)
            return null;

        return new PlayerDetails((uint)arr[0], (int)arr[1], (int)arr[2]);
    }
}