using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientServerTogglables : MonoBehaviour
{
    [Header("Local References")]
    public GameObject m_LocalTogglable;
    public Component[] m_LocalTogglableScripts;

    [Header("Network References")]
    public GameObject m_NetworkTogglable;
    public Component[] m_NetworkTogglableScripts;

    public void UpdateOwner(bool isOwner)
    {
        if (isOwner)
        {
            Destroy(m_NetworkTogglable);
            m_LocalTogglable.SetActive(true);
        }
        else
        {
            Destroy(m_LocalTogglable);
            m_NetworkTogglable.SetActive(true);
        }

        foreach (Component script in m_LocalTogglableScripts)
        {
            if (!isOwner)
                Destroy(script);
        }
        
        foreach (Component script in m_NetworkTogglableScripts)
        {
            if (isOwner)
                Destroy(script);
        }
    }
}
