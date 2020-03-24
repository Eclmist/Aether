
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class GroundSpellSkillItem : SkillItem
{
    [SerializeField]
    private float m_Speed;

    // Create 10 spikes per cast
    private const int m_MAX_SPIKES = 10;
    private int m_SpikesCounter = 0;
    private float m_Timer;
    private float m_SpawnDistance = 1f;

    public override void UseSkill(Transform playerTransform)
    {
        StartCoroutine(SpawnAllSpikes(playerTransform));
    }

    private IEnumerator SpawnAllSpikes(Transform playerTransform)
    {
        while (m_SpikesCounter <= m_MAX_SPIKES)
        {
            if (m_Timer * m_Speed >= m_SpawnDistance)
            {

                // Spawn a spike
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward.normalized * m_SpikesCounter;
                Skills spike = NetworkManager.Instance.InstantiateSkills(index: 5, position: spawnPosition, rotation: Quaternion.identity) as Skills;

                // Improve this implementation with separate function
                spike.transform.localScale += new Vector3(0.12f * m_SpikesCounter, 0.12f * m_SpikesCounter, 0.12f * m_SpikesCounter);
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
