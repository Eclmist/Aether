using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class AiMonster : AiActor, Attacker
{
    [SerializeField] 
    private AiAnimation m_MonsterAnimation;

    private HealthHandler m_HealthHandler;

    private bool m_CanAttack = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            m_NearestPlayer = other.transform;
            SetNearPlayer();
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        { 
            m_StateMachineAnim.SetBool("nearPlayer", false);
        }
    }

    public void Attack(float attackInterval)
    {
        if (m_CanAttack)
        {
            float attack = m_MonsterAnimation.RandomizeAttack();
            
            //logic for damaging the player here
            DamagePlayer();
            
            StartCoroutine(SetCanAttack(attack));
            m_CanAttack = false;
        }

        IEnumerator SetCanAttack(float delay)
        {
            yield return new WaitForSeconds(attackInterval + delay); //Divide by 2 for now
            m_CanAttack = true;
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

    private void Start()
    {
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;

        m_HealthHandler = GetComponent<HealthHandler>();
        if (m_HealthHandler == null)
        {
            Debug.LogError("No damage system, won't be able to damage players");
        }
        m_HealthHandler.HealthChanged += OnHealthChanged;
        m_HealthHandler.HealthDepleted += m_MonsterAnimation.Death;
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
        
    }

    private void OnHealthChanged(float deltaHealth)
    {
        if (deltaHealth < 0)
            m_MonsterAnimation.TakenDamage();
    }

    private void OnDestroy()
    {
        if (m_HealthHandler != null)
        {
            m_HealthHandler.HealthChanged -= OnHealthChanged;

            if (m_MonsterAnimation != null)
                m_HealthHandler.HealthDepleted -= m_MonsterAnimation.Death;
        }
    }
}