using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be attached to any game object with collider that can damage other player/neutral.
public class DamageSystem : MonoBehaviour
{
    [SerializeField]
    private float m_PureDamage = 20f;

    protected bool m_isActive = true;

    public float GetDamage()
    {
        return m_PureDamage;
    }

    public void SetDamage(float damage)
    {
        m_PureDamage = damage;
    }

    public void SetIsWeaponActive(bool isActive)
    {
        m_isActive = isActive;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (m_isActive &&  damageable != null)
        {
            // Damage calculation.
            // As of now, only pure damage dealt, however each players should have defence stat that can reduce this damage dealt.
            damageable.DamageHealth(m_PureDamage);
        }
    }
}
