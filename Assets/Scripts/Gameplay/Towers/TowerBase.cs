using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class TowerBase : TowerBehavior
{
    public const float MAXIMUM_CAPTURE_GAUGE = 100;

    public System.Action<TowerBase> TowerCaptured;

    private bool m_isBeingCaptured;

    private float m_CaptureGauge = 0;
    private bool m_IsCaptured = false;

    public float GetCaptureGauge()
    {
        return m_CaptureGauge;
    }

    public bool GetBeingCaptured()
    {
        return m_isBeingCaptured;
    }

    public void UpdateCaptureGauge(float captureGauge)
    {
        if (captureGauge < MAXIMUM_CAPTURE_GAUGE)
        {
            m_CaptureGauge = captureGauge;
        }
        else
        {
            if (captureGauge > MAXIMUM_CAPTURE_GAUGE)
                m_CaptureGauge = MAXIMUM_CAPTURE_GAUGE;
            else if (captureGauge < -MAXIMUM_CAPTURE_GAUGE)
                m_CaptureGauge = -MAXIMUM_CAPTURE_GAUGE;

            m_IsCaptured = true;
            TowerCaptured?.Invoke(this);
        }
    }

    public bool IsCaptured()
    {
        return m_IsCaptured;
    }
}
