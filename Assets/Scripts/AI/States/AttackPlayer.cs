using System.Collections;
using UnityEngine;

/*
 * Only for AiActors which implement attacker
 */
public class AttackPlayer : AiStateBehaviour
{
    [SerializeField] 
    private float m_AttackInterval = 2f;
    [SerializeField] [Range(0, 50)]
    private float m_AttackRange = 3f;
    [SerializeField]
    public float m_stateDuration = 10f;
    [SerializeField] 
    private float m_MaxDistanceFromSpawn = 65f;
    
    private Attacker m_Attacker;
    private float m_timeCounter;
   
    public override void Init()
    { 
        m_Attacker = m_AiActor as Attacker;
    }

    public override void Update()
    {
        m_timeCounter += Time.deltaTime;
    }
    
    public override void CoroutineUpdate()
    {
        if (m_Attacker == null)
        {
            Debug.LogWarning("Trying to attack without implementing attacker");
            return;
        }
        float dist = Vector3.Distance(m_NearestPlayer.position, m_AiActor.gameObject.transform.position)
        
        if (m_AttackRange > dist)
        {
            m_Attacker.Attack(m_AttackInterval);
        }
        else
        {
            m_Agent.SetDestination(m_NearestPlayer.position);
        }
        
        if (m_timeCounter > m_stateDuration)
        {
            //should I use fuzzy logic here?
            if (m_AiActor.DistanceFromSpawnPoint() > m_MaxDistanceFromSpawn || dist > 10f)
            {
                m_StateAnimator.SetTrigger("retreat");
                m_StateAnimator.SetBool("nearPlayer", false);
            }
            m_timeCounter = 0;
        }
    }
}
