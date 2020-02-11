using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected const float m_BuffDuration = 5.0f;

    public void Interact(ICanInteract interactor) 
    {
        if (interactor != null && interactor is Player player)
        {
            PowerupHandler powerupHandler = player.gameObject.GetComponent<PowerupHandler>();
            if (powerupHandler != null)
            {
                PlayPickUpSound();
                HandlePowerup(powerupHandler, player.GetPlayerMovement());
                Destroy(gameObject);
            }
        }
    }

    private void PlayPickUpSound()
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public abstract void HandlePowerup(PowerupHandler powerupHandler, PlayerMovement playerMovement);

    public abstract void OnPowerupActivated(PlayerMovement playerMovement);

    public abstract void OnPowerupExpired(PlayerMovement playerMovement);

    protected abstract IEnumerator StartPowerup(PowerupHandler powerupHandler, PlayerMovement playerMovement);
}
