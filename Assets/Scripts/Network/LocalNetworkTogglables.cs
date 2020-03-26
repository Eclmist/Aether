using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalNetworkTogglables : MonoBehaviour
{
    [Header("Local References")]
    public Component[] m_LocalComponents;

    [Header("Network References")]
    public Component[] m_NetworkComponents;

    public void UpdateOwner(bool isOwner)
    {
        foreach (Component comp in m_LocalComponents)
        {
            if (!isOwner)
                Destroy(comp);
        }
        
        foreach (Component comp in m_NetworkComponents)
        {
            if (isOwner)
                Destroy(comp);
        }
    }
}
