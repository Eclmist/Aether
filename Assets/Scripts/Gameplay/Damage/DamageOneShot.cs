using System.Collections.Generic;

public class DamageOneShot : DamageDealerBase
{
    public HashSet<HealthHandler> m_PreviousInteractions;

    private void Awake()
    {
        m_PreviousInteractions = new HashSet<HealthHandler>();
    }

    public override void DealDamage(HealthHandler healthHandler, InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                if (!m_PreviousInteractions.Contains(healthHandler))
                {
                    m_PreviousInteractions.Add(healthHandler);
                    healthHandler.Reduce(m_DamageAmount);
                }
                break;
            default:
                break;
        }
    }
}
