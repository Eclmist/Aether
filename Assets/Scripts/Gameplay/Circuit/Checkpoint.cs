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

        if (!(interactor is Player))
            return;

        Player player = interactor as Player;
        if (player != PlayerManager.Instance.GetLocalPlayer())
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

    public void SetCheckpoint()
    {
        Debug.Log("Checkpoint activated");
        m_IsActivated = false;
        GameManager.Instance.SetRespawnPoint(m_RespawnPoint);
    }
}

