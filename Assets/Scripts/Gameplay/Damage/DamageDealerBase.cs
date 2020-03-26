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
    protected float m_Duration = 0.0f;

    private bool m_IsActivated = false;

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_IsActivated = true;
        StartCoroutine(DestroyDamageDealer());
    }

    public abstract void DealDamage(HealthHandler health, InteractionType interactionType);

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
        if (networkObject == null)
            return true;

        Player owner = PlayerManager.Instance.GetPlayerById(networkObject.Owner.NetworkId);

        // Non-players should not damage each other, may need to have specific cases
        if (owner == null && !(mb is Player))
            return true;

        // Both are players, check teams
        if (owner != null && mb is Player player)
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
