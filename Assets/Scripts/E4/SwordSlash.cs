using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

/**
 * This class acts as an identifier for the NetworkManager 
 * to determine what needs to be spawned
 * when a specific ItemSkill is called
 */

public class SwordSlash : SwordSlashBehavior
{
    [SerializeField]
    private DamageDealerBase m_DamageDealerBase;

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_DamageDealerBase.Activate(networkObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        // E4 HACK
        if (other.GetComponent<AiMonster>())
            AudioManager.m_Instance.PlaySoundAtPosition("GEN_Sword_Impact_1", transform.position);
    }
}
