using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;

public abstract class DamageDealerBase : MonoBehaviour, IInteractable
{
    protected NetworkObject m_NetworkObject;

    [SerializeField]
    protected float m_DamageAmount = 0.0f;
    [SerializeField]
    protected float m_Duration = 0.0f;

    private bool m_IsActivated = false;

    public void Activate(NetworkObject networkObject)
    {
        m_NetworkObject = networkObject;
        StartCoroutine(StartupDamageDealer());
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

        if (healthHandler == null || ShouldAvoidDamage(mb))
            return;

        DealDamage(healthHandler, interactionType);
    }

    private bool ShouldAvoidDamage(MonoBehaviour mb)
    {
        if (m_NetworkObject == null)
            return true;

        Player owner = PlayerManager.Instance.GetPlayerById(m_NetworkObject.Owner.NetworkId);

        Debug.Log("Check if Player " + mb);
        // Non-players should not damage each other, may need to have specific cases
        if (owner == null && !(mb is Player))
            return true;

        // Both are players, check teams
        if (owner != null && mb is Player player)
        {
            Debug.Log("Check teams");
            Team ownerTeam = owner.GetPlayerDetails().GetTeam();
            Team playerTeam = player.GetPlayerDetails().GetTeam();

            return ownerTeam.Equals(playerTeam);
        }

        return false;
    }

    private IEnumerator StartupDamageDealer()
    {
        m_IsActivated = true;
        yield return new WaitForSeconds(m_Duration);
        m_IsActivated = false;
    }
}
