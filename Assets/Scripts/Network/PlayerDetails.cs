public class PlayerDetails
{
    private string m_Name;
    private uint m_NetworkId;
    private int m_Position;
    private ulong m_Customization;

    public PlayerDetails(string name, uint networkId, int position, ulong customization)
    {
        m_Name = name;
        m_NetworkId = networkId;
        m_Position = position;
        m_Customization = customization;
    }

    public void SetName(string name)
    {
        m_Name = name;
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

    public string GetName()
    {
        return m_Name;
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
        return new object[] { m_Name, m_NetworkId, m_Position, m_Customization };
    }

    public static PlayerDetails FromArray(object[] arr)
    {
        if (arr.Length != 4)
            return null;

        return new PlayerDetails((string)arr[0], (uint)arr[1], (int)arr[2], (ulong)arr[3]);
    }
}