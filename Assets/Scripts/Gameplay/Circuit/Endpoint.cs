using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour, IInteractable
{
    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!GameManager.Instance.GetGameStarted())
            return;

        if (!(interactor is Player) || PlayerManager.Instance.GetLocalPlayer() != (interactor as Player))
            return;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                RequestGameOver();
                break;
            default:
                break;
        }
    }

    private void RequestGameOver()
    {
        UIManager.Instance.UINotifyHeader("Test");
        GameManager.Instance.RequestGameOver();
    }
}
