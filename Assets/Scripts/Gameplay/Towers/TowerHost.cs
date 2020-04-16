using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TowerBase))]
public class TowerHost : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float m_CapturePerPaxPerSecond = 2;

    private TowerBase m_Tower;

    private List<Player> m_PlayersInCaptureZone;

    private int m_CaptureCountChangeThisFrame = 0;

    private void Awake()
    {
        m_PlayersInCaptureZone = new List<Player>();
    }

    private void Start()
    {
        m_Tower = GetComponent<TowerBase>();
    }

    private void Update()
    {
        if (!m_Tower.GetIsActivated())
            return;

        if (m_Tower.IsCaptured())
            return;

        // Current rate of gauge shift
        int captureMultiplier = m_PlayersInCaptureZone.Count;

        float captureGauge = m_Tower.GetCaptureGauge();
        captureGauge += captureMultiplier * m_CapturePerPaxPerSecond * Time.deltaTime;
        m_Tower.UpdateCaptureGauge(captureGauge);
    }

    private void LateUpdate()
    {
        m_CaptureCountChangeThisFrame = 0;
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!m_Tower.GetIsActivated())
            return;

        if (!(interactor is Player))
            return;
        Player player = interactor as Player;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                HandleEntry(player);
                break;
            case InteractionType.INTERACTION_TRIGGER_EXIT:
                HandleExit(player);
                break;
            default:
                break;
        }
    }

    public int GetCaptureCount()
    {
        return m_PlayersInCaptureZone.Count;
    }

    public int GetCaptureCountChange()
    {
        return m_CaptureCountChangeThisFrame;
    }

    private void HandleEntry(Player player)
    {
        if (!m_PlayersInCaptureZone.Contains(player))
        {
            m_PlayersInCaptureZone.Add(player);
            m_CaptureCountChangeThisFrame++;
            if (player == PlayerManager.Instance.GetLocalPlayer())
                m_Tower.TowerEntered?.Invoke(m_Tower);
        }
    }

    private void HandleExit(Player player)
    {
        if (m_PlayersInCaptureZone.Contains(player))
        {
            m_PlayersInCaptureZone.Remove(player);
            m_CaptureCountChangeThisFrame--;
            if (player == PlayerManager.Instance.GetLocalPlayer())
                m_Tower.TowerExited?.Invoke();
        }
    }
}
