using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnderGround : AiStateBehaviour
{
    [SerializeField] public float m_stateDuration = 15f;
    [SerializeField] private float m_MaxDistanceFromSpawn = 45f;
    [SerializeField] [Range(0, 50)] private float m_AttackRange = 3f;
    
    private float m_timeCounter;

    public override void Init()
    {
    }

    public override void Update()
    {
        m_timeCounter += Time.deltaTime;
    }

    public override void CoroutineUpdate()
    {
        if (m_AttackRange > Vector3.Distance(m_NearestPlayer.position, m_AiActor.gameObject.transform.position))
        {
            m_Agent.SetDestination(m_AiActor.gameObject.transform.position);
            m_StateAnimator.SetBool("nearPlayer", true);
        }
        else if (m_timeCounter > m_stateDuration)
        {
            //should I use fuzzy logic here?
            if (m_AiActor.DistanceFromSpawnPoint() > m_MaxDistanceFromSpawn)
            {
                m_StateAnimator.SetTrigger("retreat");
                m_StateAnimator.SetBool("nearPlayer", false);
            }

            m_timeCounter = 0;
        }
        else
            m_Agent.SetDestination(m_NearestPlayer.position);
    }
}
