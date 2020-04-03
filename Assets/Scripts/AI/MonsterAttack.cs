
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class MonsterAttack : MonsterAttackBehavior
{
  
    [SerializeField] private DamageDealerBase m_DamageDealerBase;

    protected override void NetworkStart()
    {
        base.NetworkStart();
        m_DamageDealerBase.Activate(networkObject, null);
    }

    public void OverrideDamageAndRadius(float damage, float radius, float duration)
    {
        m_DamageDealerBase.SetDuration(duration);
        m_DamageDealerBase.SetDamageAmount(damage);
        SphereCollider collider = m_DamageDealerBase.GetComponent<SphereCollider>();
        if (collider != null)
            collider.radius = radius;
    }
    
}
