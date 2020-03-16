using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Put script into SwordDanceEnd, for creation of sword craters when
 * swords collide into the level.
 */
public class SwordCollisionEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject m_EffectOnCollision;
    [SerializeField]
    private float m_TimeDelayBeforeDestroy;
    private ParticleSystem m_LandingSwords;
    private List<ParticleCollisionEvent> m_Collisions = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    void Start()
    {
        m_LandingSwords = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int collisionEvents = m_LandingSwords.GetCollisionEvents(other, m_Collisions);
        for (int i = 0; i < collisionEvents; i++)
        {
            GameObject collisionEffectInstance = Instantiate(m_EffectOnCollision, m_Collisions[i].intersection, new Quaternion());
            collisionEffectInstance.transform.parent = transform;
            Destroy(collisionEffectInstance, m_TimeDelayBeforeDestroy);
        }
    }
}
