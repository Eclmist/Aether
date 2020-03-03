using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(ClientServerTogglables))]
public class TowerBase : TowerBehavior
{
    public const float MAXIMUM_CAPTURE_GAUGE = 100;

    public System.Action<TowerBase> TowerCaptured;

    private ClientServerTogglables m_ClientServerTogglables;

    private float m_CaptureGauge = 0;
    private bool m_IsCaptured = false;

    private void Awake()
    {
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsServer);
    }

    public float GetCaptureGauge()
    {
        return m_CaptureGauge;
    }

    public void UpdateCaptureGauge(float captureGauge)
    {
        if (captureGauge < MAXIMUM_CAPTURE_GAUGE && captureGauge > -MAXIMUM_CAPTURE_GAUGE)
        {
            m_CaptureGauge = captureGauge;
        }
        else
        {
            if (captureGauge > MAXIMUM_CAPTURE_GAUGE)
                m_CaptureGauge = MAXIMUM_CAPTURE_GAUGE;
            else if (captureGauge < -MAXIMUM_CAPTURE_GAUGE)
                m_CaptureGauge = -MAXIMUM_CAPTURE_GAUGE;

            TowerCaptured(this);
        }
    }

    public bool IsCaptured()
    {
        return m_IsCaptured;
    }

    public void SetIsCaptured(bool isCaptured)
    {
        m_IsCaptured = isCaptured;
    }

    public CaptureState GetCaptureState()
    {
        return new CaptureState(m_CaptureGauge);
    }

    public struct CaptureState
    {
        private Team m_LeadingTeam;
        private float m_CapturePercentage; 

        public CaptureState(float captureGauge)
        {
            // Only supports two teams using 1 single float as gauge
            m_LeadingTeam = captureGauge > 0 ? Team.TEAM_ONE : Team.TEAM_TWO;
            m_CapturePercentage = captureGauge < 0 ? -captureGauge : captureGauge;
        }

        public float GetCapturePercentage()
        {
            return m_CapturePercentage;
        }

        public Team GetLeadingTeam()
        {
            return m_LeadingTeam;
        }
    }
}
