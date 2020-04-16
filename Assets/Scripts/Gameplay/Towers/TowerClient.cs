using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TowerBase))]
public class TowerClient : MonoBehaviour, IInteractable
{
    private TowerBase m_Tower;

    private void Start()
    {
        m_Tower = GetComponent<TowerBase>();
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!m_Tower.GetIsActivated())
            return;

        if (!(interactor is Player))
            return;

        Player player = interactor as Player;
        if (player != PlayerManager.Instance.GetLocalPlayer())
            return;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                HandleEntry();
                break;
            case InteractionType.INTERACTION_TRIGGER_EXIT:
                HandleExit();
                break;
            default:
                break;
        }
    }

    private void HandleEntry()
    {
        m_Tower.TowerEntered?.Invoke(m_Tower);
    }

    private void HandleExit()
    {
        m_Tower.TowerExited?.Invoke();
    }
}
