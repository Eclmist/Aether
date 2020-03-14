using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Debug = System.Diagnostics.Debug;

[RequireComponent(typeof(Collider))]
public class AiMonster : AiActor, Attacker
{
    [SerializeField] 
    private Animator m_PlantMonsterAnimator;
    [SerializeField]
    private float m_farAwayDistance = 10f;

    private enum AnimMovesParam
    {
        locomotion,
        attack1 = 1,
        attack2 = 2,
        idleBreak,
        death,
        gotHit,
        goPlant,
        goAlive
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            m_NearestPlayer = other.transform;
            SetNearPlayer();
        }
    }

    bool canAttack = true;
    
    public void Attack(float attackInterval)
    {
        if (canAttack)
        {
            AnimMovesParam attack = RandomizeAttack();
            
            //logic for damaging the player here
            DamagePlayer();
            
            
            m_PlantMonsterAnimator.SetTrigger(attack.ToString());
            StartCoroutine(SetCanAttack((int) attack));
            canAttack = false;
        }

        IEnumerator SetCanAttack(float delay)
        {
            yield return new WaitForSeconds(attackInterval + delay);
            canAttack = true;
        }
    }

    private AnimMovesParam RandomizeAttack()
    {
        AnimMovesParam [] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2};
        return temp[Random.Range(0, temp.Length)];
    }

    private void DamagePlayer()
    {
    }

    private void SetNearPlayer()
    {
        //alerts the animator if the player has entered the vicinity.
        m_StateMachineAnim.SetBool("nearPlayer", true);
        m_PlantMonsterAnimator.SetTrigger(AnimMovesParam.goAlive.ToString());
    }

    public void Start()
    {
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;
    }

    private void RotateTowardsNearestPlayer()
    {
        if (m_NearestPlayer == null)
        {
            return;
        }
        Vector3 direction = (m_NearestPlayer.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_Agent.angularSpeed);
    }

    public override void SetInactive()
    {
        m_PlantMonsterAnimator.SetTrigger(AnimMovesParam.goPlant.ToString());
        base.SetInactive();
    }

    public void Update()
    {
        if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            m_PlantMonsterAnimator.SetFloat("locomotion", 0.55f); //hardcoded value now as it's the most appealing
        }
        else
        {
            m_PlantMonsterAnimator.SetFloat("locomotion", 0);
            if (canAttack)
            {
                RotateTowardsNearestPlayer();
            }
        }
        
        if (m_NearestPlayer == null)
        {
            return;
        }

        if (Vector3.Distance(m_NearestPlayer.position, transform.position) > m_farAwayDistance)
        {
            m_StateMachineAnim.SetBool("nearPlayer", false);
        }
    }
    
}