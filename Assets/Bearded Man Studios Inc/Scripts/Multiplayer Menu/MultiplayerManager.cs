using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : Singleton<MultiplayerManager>
{
    [SerializeField]
    private MultiplayerMenu m_Menu;

    public void SaveIPAddress(string ipAddress)
    {
        m_Menu.ipAddress = ipAddress;
    }

    public string GetIPAddress()
    {
        return m_Menu.ipAddress;
    }

    public void SavePortNumber(string portNumber)
    {
        m_Menu.portNumber = portNumber;
    }

    public string GetPortNumber()
    {
        return m_Menu.portNumber;
    }
}
