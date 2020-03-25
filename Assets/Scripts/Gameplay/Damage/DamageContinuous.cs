using System.Collections.Generic;
using UnityEngine;

public class DamageContinuous : DamageDealerBase
{
    [SerializeField]
    private float m_DamageInterval = 0.1f;

    private HashSet<HealthHandler> m_Targets;

    private float m_TimeLastDamaged;

    private void Awake()
    {
        m_Targets = new HashSet<HealthHandler>();
    }

    private void Update()
    {
        if (Time.time >= m_TimeLastDamaged + m_DamageInterval)
        {
            foreach (HealthHandler target in m_Targets)
            {
                target.Damage(m_DamageAmount);
            }

            m_TimeLastDamaged = Time.time;
        }
    }

    public override void DealDamage(HealthHandler healthHandler, InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.INTERACTION_TRIGGER_ENTER:
                m_Targets.Add(healthHandler);
                break;
            case InteractionType.INTERACTION_TRIGGER_EXIT:
                m_Targets.Remove(healthHandler);
                break;
            default:
                break;
        }
    }
}
