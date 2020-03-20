using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class DamageDealerBase : MonoBehaviour, IInteractable
{
    protected float m_DamageAmount = 0.0f;
    protected float m_Radius = 0.0f;
    private float m_Duration = 0.0f;

    private bool m_IsActivated = false;

    public abstract void DealDamage(HealthHandler health, InteractionType interactionType);

    public void ActivateDamage()
    {
        m_IsActivated = true;
        StartCoroutine(DestroyDamageDealer());
    }

    public void InitializeDamageDealer(float damage, float radius, float duration)
    {
        m_DamageAmount = damage;
        m_Radius = radius;
        m_Duration = duration;
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (!m_IsActivated)
            return;
        // Interactor should be a monobehavior
        // Implementer's fault if error is thrown
        MonoBehaviour mb = interactor as MonoBehaviour;

        HealthHandler healthHandler = mb.GetComponent<HealthHandler>();
        if (healthHandler != null)
            DealDamage(healthHandler, interactionType);
    }

    private IEnumerator DestroyDamageDealer()
    {
        yield return new WaitForSeconds(m_Duration);
        Destroy(gameObject);
    }
}
