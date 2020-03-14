using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be attached to any game object with collider that can damage other player/neutral.
public class DamageSystem : MonoBehaviour
{
    [SerializeField]
    private double m_PureDamage;

    void OnTriggerEnter(Collider other)
    {
        // For now will do checks by Tags, probably a better way to handle this. 
        // Also, only applicable to players for now since neutrals not implemented yet.
        if (other.GetComponent<Player>() != null)
        {
            // Damage calculation.
            // As of now, only pure damage dealt, however each players should have defence stat that can reduce this damage dealt.
            Player player = other.GetComponent<Player>();
            player.DamageHealth(m_PureDamage);
        }
    }
}
