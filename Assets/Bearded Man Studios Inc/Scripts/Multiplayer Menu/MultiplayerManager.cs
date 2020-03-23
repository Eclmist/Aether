using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : Singleton<MultiplayerManager>
{
    [SerializeField]
    private MultiplayerMenu m_Menu;

    public void SaveIPAddress(string ipAddress)
    {
        m_Menu.ipAddress.text = ipAddress;
    }

    public string GetIPAddress()
    {
        return m_Menu.ipAddress.text;
    }

    public void SavePortNumber(string portNumber)
    {
        m_Menu.portNumber.text = portNumber;
    }

    public string GetPortNumber()
    {
        return m_Menu.portNumber.text;
    }
}
