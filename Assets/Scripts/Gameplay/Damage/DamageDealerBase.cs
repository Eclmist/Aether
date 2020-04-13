using System.Collections;
using UnityEngine;

public abstract class DamageDealerBase : MonoBehaviour, IInteractable
{
    // event keyword omitted to allow subclass to call
    public System.Action PlayDamageSound;

    [SerializeField]
    protected float m_StartOffset = 0.0f;
    [SerializeField]
    protected float m_DamageAmount = 0.0f;
    [SerializeField]
    protected float m_Duration = 0.0f;
    [SerializeField]
    protected bool m_IsPlayerAttack = true;

    private bool m_IsActivated = false;

    public void Activate(System.Action soundCallback)
    {
        StartCoroutine(StartupDamageDealer());
        PlayDamageSound += soundCallback;
    }

    public abstract void DealDamage(HealthHandler health, InteractionType interactionType);

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!m_IsActivated)
            return;

        // Interactor should be a monobehavior
        // Implementer's fault if error is thrown
        MonoBehaviour mb = interactor as MonoBehaviour;
        HealthHandler healthHandler = mb.GetComponent<HealthHandler>();

        if (healthHandler == null || ShouldAvoidDamage(interactor))
            return;

        DealDamage(healthHandler, interactionType);
    }

    private bool ShouldAvoidDamage(ICanInteract interactor)
    {
        // If interactor is player, avoid getting hit by player attacks.
        // If interactor is monster, avoid getting hit by non-player attacks.
        return (interactor is Player) == m_IsPlayerAttack;
    }

    private IEnumerator StartupDamageDealer()
    {
        yield return new WaitForSeconds(m_StartOffset);
        m_IsActivated = true;
        yield return new WaitForSeconds(m_Duration);
        m_IsActivated = false;
        Destroy(gameObject);
    }
}
