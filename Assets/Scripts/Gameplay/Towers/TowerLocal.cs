using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TowerLocal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TowerBase m_Tower;

    [SerializeField]
    private float m_CapturePerPaxPerSecond = 2;

    private List<Player> m_PlayersInCaptureZone;

    private void Awake()
    {
        m_PlayersInCaptureZone = new List<Player>();
    }

    private void Update()
    {
        if (m_Tower.IsCaptured())
            return;

        // Check current rate of gauge shift
        int captureMultiplier = 0;

        foreach (Player player in m_PlayersInCaptureZone)
        {
            Team team = player.GetPlayerDetails().GetTeam();
            switch (team)
            {
                case Team.TEAM_ONE:
                    captureMultiplier++;
                    break;
                case Team.TEAM_TWO:
                    captureMultiplier--;
                    break;
                default:
                    Debug.Assert(false, "Should not be reached unless a team was unhandled. TowerLocal.Update");
                    break;
            }
        }

        float captureGauge = m_Tower.GetCaptureGauge();
        captureGauge += captureMultiplier * m_CapturePerPaxPerSecond * Time.deltaTime;
        m_Tower.UpdateCaptureGauge(captureGauge);

        // Update network data
        if (m_Tower.networkObject != null)
            m_Tower.networkObject.captureGauge = captureGauge;
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
            m_PlayersInCaptureZone.Add(player);
    }

    private void HandleExit(Player player)
    {
        if (m_PlayersInCaptureZone.Contains(player))
            m_PlayersInCaptureZone.Remove(player);
    }
}
