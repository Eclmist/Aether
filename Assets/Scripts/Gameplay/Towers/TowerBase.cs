using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;

[RequireComponent(typeof(LocalNetworkTogglables))]
[RequireComponent(typeof(TowerNetworkHandler))]
public class TowerBase : TowerBehavior
{
    public const float MAXIMUM_CAPTURE_GAUGE = 100;

    public event System.Action<TowerBase> TowerCaptured;
    public System.Action<TowerBase> TowerEntered;
    public System.Action TowerExited;

    [SerializeField]
    private TowerBase m_NextTower;

    [SerializeField] // Higher priority level = will activate
    private int m_PriorityLevel = 0;

    private LocalNetworkTogglables m_LocalNetworkTogglables;
    private TowerNetworkHandler m_TowerNetworkHandler;

    private bool m_isBeingCaptured;

    private float m_CaptureGauge = 0;
    private bool m_IsCaptured = false;

    private void Awake()
    {
        m_LocalNetworkTogglables = GetComponent<LocalNetworkTogglables>();
        m_TowerNetworkHandler = GetComponent<TowerNetworkHandler>();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_LocalNetworkTogglables.UpdateOwner(networkObject.IsServer);
    }

    public void RevealNext()
    {
        if (m_NextTower != null)
            m_NextTower.gameObject.SetActive(true);
    }

    public float GetCapturePercentage()
    {
        return m_CaptureGauge / MAXIMUM_CAPTURE_GAUGE;
    }

    public float GetCaptureGauge()
    {
        return m_CaptureGauge;
    }

    public bool GetIsCaptured()
    {
        return m_IsCaptured;
    }

    public int GetPriority()
    {
        return m_PriorityLevel;
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

    public override void SignalEntry(RpcArgs args)
    {
        m_TowerNetworkHandler.SignalEntry(args.GetNext<int>());
    }

    public override void SignalExit(RpcArgs args)
    {
        m_TowerNetworkHandler.SignalExit(args.GetNext<int>());
    }
}
