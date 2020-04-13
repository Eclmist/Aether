using UnityEngine;

[RequireComponent(typeof(TowerBase))]
public class TowerNetwork : MonoBehaviour
{
    private TowerBase m_Tower;

    private void Start()
    {
        m_Tower = GetComponent<TowerBase>();
    }

    private void Update()
    {
        // Retrieve network data
        if (m_Tower.networkObject != null)
            m_Tower.UpdateCaptureGauge(m_Tower.networkObject.captureGauge);
    }
}
