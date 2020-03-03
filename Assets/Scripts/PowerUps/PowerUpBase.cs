using System.Reflection;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected const float m_BuffDuration = 5.5f;

    protected float m_TimeOfActivation = -1.0f;

    void Update()
    {
        if (m_TimeOfActivation != -1.0f && Time.time > m_TimeOfActivation + m_BuffDuration)
        {
            OnPowerUpExpired();
            Destroy(this);
        }
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType) 
    {
        if (!(interactor is Player))
            return;
        Player player = interactor as Player;

        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
                StartActivation();
                HandlePowerUp(player);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void StartActivation()
    {
        m_TimeOfActivation = Time.time;
        OnPowerUpActivated();
    }

    public void HandlePowerUp(Player player)
    {
        System.Type powerUpType = GetType();
        if (player.GetComponent(powerUpType) == null)
        {
            PowerUpBase powerUp = (PowerUpBase)player.gameObject.AddComponent(powerUpType);
            CopyPowerUpToPlayer(this, powerUp); // Use reflection to copy script onto player

            powerUp.m_TimeOfActivation = Time.time;
            powerUp.OnPowerUpActivated();
        }
    }

    protected static void CopyPowerUpToPlayer(PowerUpBase source, PowerUpBase target)
    {
        FieldInfo[] sourceFields = source.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < sourceFields.Length; i++)
        {
            var value = sourceFields[i].GetValue(source);
            sourceFields[i].SetValue(target, value);
        }
    }

    public abstract void OnPowerUpActivated();

    public abstract void OnPowerUpExpired();
}
