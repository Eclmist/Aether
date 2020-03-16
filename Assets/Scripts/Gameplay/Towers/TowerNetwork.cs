using UnityEngine;

[RequireComponent(typeof(LocalNetworkTogglables))]
public class TowerNetwork : MonoBehaviour
{
    [SerializeField]
    private TowerBase m_Tower;

    private void Update()
    {
        // Retrieve network data
        if (m_Tower.networkObject != null)
            m_Tower.UpdateCaptureGauge(m_Tower.networkObject.captureGauge);
    }
}
