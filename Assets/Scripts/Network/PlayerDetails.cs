public class PlayerDetails
{
    private uint m_NetworkId;
    private int m_Position;
    private ulong m_Customization;

    public PlayerDetails(uint networkId, int position, ulong customization)
    {
        m_NetworkId = networkId;
        m_Position = position;
        m_Customization = customization;
    }

    public void SetNetworkId(uint networkId)
    {
        m_NetworkId = networkId;
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
        return new object[] { m_NetworkId, m_Position, m_Customization };
    }

    public static PlayerDetails FromArray(object[] arr)
    {
        if (arr.Length != 3)
            return null;

        return new PlayerDetails((uint)arr[0], (int)arr[1], (ulong)arr[2]);
    }
}