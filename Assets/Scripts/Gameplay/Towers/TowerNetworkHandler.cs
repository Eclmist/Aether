using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
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
            if (NetworkManager.Instance.IsServer)
            {
                if (m_TowerHost == null)
                    return;

                m_Tower.networkObject.captureGauge = m_Tower.GetCaptureGauge();

                int change = m_TowerHost.GetCaptureCountChange();
                int currCount = m_TowerHost.GetCaptureCount();
                if (change > 0)
                    m_Tower.networkObject.SendRpc(TowerBase.RPC_SIGNAL_ENTRY, Receivers.All, currCount);
                else if (change < 0)
                    m_Tower.networkObject.SendRpc(TowerBase.RPC_SIGNAL_EXIT, Receivers.All, currCount);
            }
            else
            {
                m_Tower.UpdateCaptureGauge(m_Tower.networkObject.captureGauge);
            }
        }
    }

    public void SignalEntry(int playerCount)
    {
        if (!m_Tower.IsCaptured())
            if (playerCount == 1)
                UIManager.Instance.UINotifySecondary("Starting checkpoint capture.");
    }

    public void SignalExit(int playerCount)
    {
        if (!m_Tower.IsCaptured())
            if (playerCount == 0)
                UIManager.Instance.UINotifySecondary("Checkpoint capture stopped.");
    }
}
