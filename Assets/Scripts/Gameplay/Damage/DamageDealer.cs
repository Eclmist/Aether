using System;
using UnityEngine;

// To be attached to any game object with collider that can damage other player/neutral.
[RequireComponent(typeof(Collider))]
public class DamageDealer : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float m_PureDamage = 20f;

    public delegate void OnHit();
    private OnHit m_onHit;
    private bool m_isActive = true;

    public void AddDamageCallback(OnHit callback)
    {
        m_onHit += callback;
    }
    
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
            m_onHit?.Invoke();
        }
    }

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {

    }
}
