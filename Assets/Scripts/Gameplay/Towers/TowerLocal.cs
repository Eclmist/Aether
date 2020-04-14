using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TowerBase))]
public class TowerLocal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float m_CapturePerPaxPerSecond = 2;

    private TowerBase m_Tower;

    private List<Player> m_PlayersInCaptureZone;

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
        if (m_Tower.IsCaptured())
            return;

        // Current rate of gauge shift
        int captureMultiplier = m_PlayersInCaptureZone.Count;

        float captureGauge = m_Tower.GetCaptureGauge();
        captureGauge += captureMultiplier * m_CapturePerPaxPerSecond * Time.deltaTime;
        m_Tower.UpdateCaptureGauge(captureGauge);
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
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

    private void HandleEntry(Player player)
    {
        if (!m_PlayersInCaptureZone.Contains(player))
        {
            m_PlayersInCaptureZone.Add(player);
            UIManager.Instance.NotifySecondary("You have started capturing the checkpoint.");
            m_Tower.TowerEntered?.Invoke(m_Tower);
        }
    }

    private void HandleExit(Player player)
    {
        if (m_PlayersInCaptureZone.Contains(player))
        {
            m_PlayersInCaptureZone.Remove(player);
            UIManager.Instance.NotifySecondary("You are no longer capturing the checkpoint.");
            GameManager.Instance.SetCurrentTower(null);
            m_Tower.TowerExited?.Invoke();
        }
    }
}
