using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform m_RespawnPoint;

    private bool m_AlreadyPassed = false;

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!GameManager.Instance.GetGameStarted())
            return;

        if (m_AlreadyPassed)
            return;

        if (!(interactor is Player) || PlayerManager.Instance.GetLocalPlayer() != (interactor as Player))
            return;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                SetCheckpoint();
                break;
            default:
                break;
        }
    }

    private void SetCheckpoint()
    {
        Debug.Log("Checkpoint activated");
        m_AlreadyPassed = true;
        GameManager.Instance.SetRespawnPoint(m_RespawnPoint);
    }
}

