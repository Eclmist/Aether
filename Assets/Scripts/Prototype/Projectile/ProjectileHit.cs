using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    // The prefab containing the particle effect when the projectile hits something.
    public GameObject hitPrefab;

    // Used to destroy the projectile when it hits something.
    void OnTriggerEnter(Collider other)
    {
        Vector3 contact = gameObject.transform.position;
        if (hitPrefab != null)
        {
            GameObject hitVFX = Instantiate(hitPrefab, contact, Quaternion.identity);
            AudioManager.m_Instance.PlaySoundAtPosition("PROJECTILE_Hit", contact);
            ParticleSystem particleSystemHit = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystemHit != null)
            {
                Destroy(hitVFX, particleSystemHit.main.duration);
            }
            else
            {
                ParticleSystem particleSystemChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (particleSystemChild != null)
                {
                    Destroy(hitVFX, particleSystemChild.main.duration);
                }
            }
        }
        
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
