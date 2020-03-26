using System.Collections.Generic;

public class DamageOneShot : DamageDealerBase
{
    public HashSet<HealthHandler> m_PreviousInteractors;

    private void Awake()
    {
        m_PreviousInteractors = new HashSet<HealthHandler>();
    }

    public override void DealDamage(HealthHandler healthHandler, InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_STAY:
                if (!m_PreviousInteractors.Contains(healthHandler))
                {
                    m_PreviousInteractors.Add(healthHandler);
                    healthHandler.Damage(m_DamageAmount);
                    PlayDamageSound?.Invoke();
                }
                break;
            default:
                break;
        }
    }
}
