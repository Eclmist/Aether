using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place on Spawner Object in player
public class FrontSpell : MonoBehaviour
{
    [SerializeField]
    private Transform m_LaunchPoint;
    [SerializeField]
    private float m_Speed = 15f;
    [SerializeField]
    private GameObject m_SpikePrefab;
    [SerializeField]
    private float m_DistanceBetweenSpawn = 1.5f;
    [SerializeField]
    private float m_SpellCastDuration = 0.5f;

    private float m_Timer = 0f;
    private Vector3 m_PreviousSpawnLocation;
    private Vector3 m_OriginalRotation = new Vector3(0, 0, 0);
    private ParticleSystem m_MainPS;
    private Vector3 m_SpellForwardDirection;
    private AudioSource m_CastAudio;

    private void Start()
    {
        m_MainPS = m_SpikePrefab.GetComponent<ParticleSystem>();
        m_CastAudio = GetComponent<AudioSource>();
    }

    /**
     * For Reference on Launching of Spike Spell
     */
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 forwardCam = Camera.main.transform.forward;
            forwardCam.y = 0;
            transform.position = m_LaunchPoint.position;
            Vector3 targetPosition = transform.position + forwardCam * 4;
            CastGroundSpell(targetPosition);
        }
    }

    private void CastGroundSpell(Vector3 targetPoint)
    {
        // Set spawn direction of ground spikes
        Vector3 lookPos = targetPoint - transform.position;
        lookPos.y = 0;
        
        // Set rotation of all spawn
        transform.rotation = Quaternion.LookRotation(lookPos);

        // Set spawn initial position
        m_PreviousSpawnLocation = transform.position;
        m_Timer = 0;
        m_SpellForwardDirection = transform.forward;
        m_CastAudio.Play();
        StartCoroutine(SpikeSpawn());
    }

    private IEnumerator SpikeSpawn()
    {
        while (true)
        {
            // Keep track of time elapsed
            m_Timer += Time.deltaTime;

            // Set how much the spawner should move forward
            transform.position += m_SpellForwardDirection * (m_Speed * Time.deltaTime);

            // Get difference between spawner position and previous spawn position
            Vector3 currentPositionDifference = transform.position - m_PreviousSpawnLocation;
            float currentDistFromPrev = currentPositionDifference.magnitude;
            
            // Spawn at every 1.5m 
            if (currentDistFromPrev > m_DistanceBetweenSpawn)
            {
                if (m_SpikePrefab != null)
                {
                    // Spawn with a random x and z offset
                    Vector3 randomOffset = new Vector3(0, 0, 0);

                    Vector3 spawnPosition = transform.position + (randomOffset * m_Timer * 2);

                    // Spawn on top of terrain in map
                    if (Terrain.activeTerrain != null)
                    {
                        spawnPosition.y = Terrain.activeTerrain.SampleHeight(transform.position);
                    }

                    GameObject spike = Instantiate(m_SpikePrefab, spawnPosition, Quaternion.identity);
                    spike.transform.localScale += new Vector3(m_Timer * 2, m_Timer * 2, m_Timer * 2);
                    Destroy(spike, m_MainPS.main.duration);
                }

                // Update the previous spawn position with the current transform position
                m_PreviousSpawnLocation = transform.position;
            }

            // Reset the spawner object to the character's position after spell cast
            if (m_Timer > m_SpellCastDuration)
            {
                transform.parent = m_LaunchPoint;
                transform.position = m_LaunchPoint.position;
                transform.rotation = Quaternion.LookRotation(m_OriginalRotation);
                yield break;
            }

            yield return null;
        }
    }
}
