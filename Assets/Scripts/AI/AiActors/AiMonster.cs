using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class AiMonster : AiActor, Attacker, Damageable
{
    [SerializeField] 
    private AiAnimation m_MonsterAnimation;
    [SerializeField]
    private float m_farAwayDistance = 10f;

    private float m_healthPoints = 100f;
    
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
            float attack = m_MonsterAnimation.RandomizeAttack();
            
            //logic for damaging the player here
            DamagePlayer();
            
            StartCoroutine(SetCanAttack(attack));
            canAttack = false;
        }

        IEnumerator SetCanAttack(float delay)
        {
            yield return new WaitForSeconds(attackInterval + delay); //Divide by 2 for now
            canAttack = true;
        }
    }
    
    private void DamagePlayer()
    {
    }

    private void SetNearPlayer()
    {
        //alerts the animator if the player has entered the vicinity.
        m_StateMachineAnim.SetBool("nearPlayer", true);
        m_MonsterAnimation.ReactToPlayer();
    }

    public void Start()
    {
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;

        DamageDealer damageDealer = gameObject.GetComponentInChildren<DamageDealer>();
        if (damageDealer == null)
        {
            Debug.LogError("No damage system, won't be able to damage players");
        }

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
        m_MonsterAnimation.GoInactive();
        base.SetInactive();
    }

    //todo: make this take in a float to adjust movement
    protected virtual void MoveMonster(bool toMove)
    {
        if (toMove)
        {
            
            m_MonsterAnimation.Move(true);
        }
        else
        {
            m_MonsterAnimation.Move(false);
        }
    }

    public void Update()
    {
        if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            MoveMonster(true);
        }
        else
        {
            MoveMonster(false);
            RotateTowardsNearestPlayer();
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

    /*
    * Called when aiMonster is attacked
    */
    public void DamageHealth(float damage)
    {
        Debug.Log("Taking Damage Monster");
        m_MonsterAnimation.TakenDamage();
        m_healthPoints -= damage;
        if (m_healthPoints < 0)
        {
            m_MonsterAnimation.Death();
        }
    }
}