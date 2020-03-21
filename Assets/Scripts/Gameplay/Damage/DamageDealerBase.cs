using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(Collider))]
public abstract class DamageDealerBase : DamageBehavior, IInteractable
{
    [SerializeField]
    protected float m_DamageAmount = 0.0f;
    [SerializeField]
    protected float m_Radius = 0.0f;
    [SerializeField]
    private float m_Duration = 0.0f;
    private MonoBehaviour m_Owner;

    private bool m_IsActivated = false;

    public abstract void DealDamage(HealthHandler health, InteractionType interactionType);

    public void ActivateDamage()
    {
        m_IsActivated = true;
        StartCoroutine(DestroyDamageDealer());
    }

    public void InitializeDamageDealer(MonoBehaviour owner, float damage, float radius, float duration)
    {
        m_Owner = owner;
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

        if (ShouldAvoidDamage(mb))
            return;

        HealthHandler healthHandler = mb.GetComponent<HealthHandler>();
        if (healthHandler != null)
            DealDamage(healthHandler, interactionType);
    }

    private bool ShouldAvoidDamage(MonoBehaviour mb)
    {
        // Should not be able to damage self
        if (mb == m_Owner)
            return true;

        // Non-players should not damage each other, may need to have specific cases
        if (!(m_Owner is Player) && !(mb is Player))
            return true;

        // Both are players, check teams
        if (m_Owner is Player owner && mb is Player player)
        {
            Team ownerTeam = owner.GetPlayerDetails().GetTeam();
            Team playerTeam = player.GetPlayerDetails().GetTeam();

            return ownerTeam.Equals(playerTeam);
        }

        return false;
    }

    private IEnumerator DestroyDamageDealer()
    {
        yield return new WaitForSeconds(m_Duration);
        Destroy(gameObject);
    }
}
