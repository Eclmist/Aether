
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class MonsterAttack : MonsterAttackBehavior
{
  
    [SerializeField] private DamageDealerBase m_DamageDealerBase;

    private void Start()
    {
        m_DamageDealerBase.Activate(null);
    }
}
