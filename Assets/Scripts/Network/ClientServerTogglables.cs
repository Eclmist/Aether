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

    void Start()
    {
        // TODO: Replace this with is owner check on network manager
        bool isOwner = false;

        if (isOwner)
            Destroy(m_NetworkTogglable);
        else
            Destroy(m_LocalTogglable);

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
