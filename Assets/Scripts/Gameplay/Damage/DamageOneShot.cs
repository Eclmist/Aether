using System.Collections.Generic;

public class DamageOneShot : DamageDealerBase
{
    public HashSet<Health> m_PreviousInteractions;

    private void Awake()
    {
        m_PreviousInteractions = new HashSet<Health>();
    }

    public override void DealDamage(Health health, InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                if (!m_PreviousInteractions.Contains(health))
                {
                    m_PreviousInteractions.Add(health);
                    health.Reduce(m_DamageAmount);
                }
                break;
            default:
                break;
        }
    }
}
