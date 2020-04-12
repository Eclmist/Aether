
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class MonsterAttack : MonsterAttackBehavior
{
  
    [SerializeField] private DamageDealerBase m_DamageDealerBase;

    protected override void NetworkStart()
    {
        base.NetworkStart();
        m_DamageDealerBase.Activate(null);
    }
}
