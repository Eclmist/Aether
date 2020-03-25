using System.Collections.Generic;
using UnityEngine;

public class DamageContinuous : DamageDealerBase
{
    [SerializeField]
    private float m_DamageInterval = 0.1f;

    private float m_TimeLastDamaged;

    public override void DealDamage(HealthHandler healthHandler, InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_STAY:
                if (Time.time >= m_TimeLastDamaged + m_DamageInterval)
                {
                    healthHandler.Damage(m_DamageAmount);
                    m_TimeLastDamaged = Time.time;
                }
                break;
            default:
                break;
        }
    }
}
