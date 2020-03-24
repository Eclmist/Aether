
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class GroundSpikeSkill : SkillsBehavior
{
    [SerializeField]
    private GameObject m_GroundSpike;
    [SerializeField]
    private float m_Speed;

    // Create 10 spikes per cast
    private const int m_MAX_SPIKES = 14;
    private int m_SpikesCounter = 0;
    private float m_Timer;
    private float m_SpawnDistance = 0.5f;

    public void SpawnSpikeSpell(Transform playerTransform)
    {
        StartCoroutine(SpawnSpike(playerTransform));
    }
    private IEnumerator SpawnSpike(Transform playerTransform)
    {
        while (m_SpikesCounter <= m_MAX_SPIKES)
        {
            if (m_Timer * m_Speed >= m_SpawnDistance)
            {

                // Spawn a spike
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward.normalized * (m_SpawnDistance * m_SpikesCounter);
                GameObject spike = Instantiate(m_GroundSpike, spawnPosition, Quaternion.identity);
                spike.transform.localScale += new Vector3(0.1f * m_SpikesCounter, 0.1f * m_SpikesCounter, 0.1f * m_SpikesCounter);
                m_Timer = 0;
                m_SpikesCounter++;
                yield return null;
            }

            else
            {
                m_Timer += Time.deltaTime;
            }

        }
        m_SpikesCounter = 0;
        yield break;
    }
}
