using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform m_RespawnPoint;

    private bool m_IsActivated = false;

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!m_IsActivated)
            return;

        if (!GameManager.Instance.GetGameStarted())
            return;

        if (!(interactor is Player) || PlayerManager.Instance.GetLocalPlayer() != (interactor as Player))
            return;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_STAY:
                SetCheckpoint();
                break;
            default:
                break;
        }
    }

    public void Activate()
    {
        m_IsActivated = true;
    }

    public void Deactivate()
    {
        m_IsActivated = false;
    }

    private void SetCheckpoint()
    {
        Debug.Log("Checkpoint activated");
        m_IsActivated = false;
        GameManager.Instance.SetRespawnPoint(m_RespawnPoint);
    }
}

