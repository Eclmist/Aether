﻿using BeardedManStudios.Forge.Networking;
using UnityEngine;

[RequireComponent(typeof(TowerBase))]
public class TowerNetworkHandler : MonoBehaviour
{
    private TowerBase m_Tower;
    private TowerHost m_TowerHost;

    private void Start()
    {
        m_Tower = GetComponent<TowerBase>();
        m_TowerHost = GetComponent<TowerHost>();
    }

    private void Update()
    {
        // Retrieve network data
        if (m_Tower.networkObject != null)
        {
            if (m_Tower.networkObject.IsServer)
            {
                if (m_TowerHost == null)
                    return;

                m_Tower.networkObject.captureGauge = m_Tower.GetCaptureGauge();

                int change = m_TowerHost.GetCaptureCountChange();
                if (change > 0)
                    m_Tower.networkObject.SendRpc(TowerBase.RPC_SIGNAL_ENTRY, Receivers.All, m_TowerHost.GetCaptureCount());
                else if (change < 0)
                    m_Tower.networkObject.SendRpc(TowerBase.RPC_SIGNAL_EXIT, Receivers.All, m_TowerHost.GetCaptureCount());
            }
            else
            {
                m_Tower.UpdateCaptureGauge(m_Tower.networkObject.captureGauge);
            }
        }
    }

    public void SignalEntry(int playerCount)
    {
        if (playerCount == 1)
            UIManager.Instance.NotifySecondary("Starting checkpoint capture at 1x speed.");
        else
            UIManager.Instance.NotifySecondary("Capture rate increased. Now at " + playerCount + "x speed.");
    }

    public void SignalExit(int playerCount)
    {
        if (playerCount == 0)
            UIManager.Instance.NotifySecondary("Checkpoint capture stopped.");
        else
            UIManager.Instance.NotifySecondary("Capture rate decreased. Now at " + playerCount + "x speed.");

    }
}
