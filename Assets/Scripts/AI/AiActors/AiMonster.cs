using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AiMonster : AiActor, Attacker, ICanInteract
{
    [SerializeField] 
    private AiAnimation m_MonsterAnimation;
    
    [SerializeField] 
    private Transform[] m_AttackSource;

    [SerializeField] 
    private float m_DamageAmount = 10f;

    [SerializeField] 
    private float m_DamageRadius = 5f;

    [SerializeField] 
    private float m_DamageDuration = 0.2f;

    private bool m_isDead = false;
    public bool DEBUG_DEATH = false;
    public bool DEBUG_GOTHIT = false;

    private HealthHandler m_HealthHandler;

    private bool m_CanAttack = true;
    private string m_AttackThisFrame = "";
    private bool m_IsStun = false;
    private SkinnedMeshRenderer[] m_MonsterSkin;
    private ParticleSystem[] m_DeathParticles;
    
    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Player>() != null)
        {
            
            if(m_NearestPlayer!=null)
            {
                if (Vector3.Distance(m_NearestPlayer.position, gameObject.transform.position) > Vector3.Distance(c.transform.position, gameObject.transform.position))
                    m_NearestPlayer = c.transform;
            } 
            else 
            {
                m_NearestPlayer = c.transform;
            }
            
            SetNearPlayer();
        }

        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_ENTER);
    }

    private void OnTriggerStay(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_STAY);
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.GetComponent<Player>() != null)
        { 
            m_StateMachineAnim.SetBool("nearPlayer", false);
        }
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_EXIT);
    }

    private void InteractWith(IInteractable interactable, InteractionType interactionType)
    {
        if (interactable != null) // null check done here instead. 
            interactable.Interact(this, interactionType);
    }

    IEnumerator SetCanAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Divide by 2 for now
        m_CanAttack = true;
    }
    public void Attack(float attackInterval)
    {
        if (m_CanAttack)
        {
            float attack = m_MonsterAnimation.RandomizeAttack(out m_AttackThisFrame)/2;
            Debug.Log("Aimonster " + m_AttackThisFrame);
            //logic for damaging the player here
            StartCoroutine(DealDamage(attack));
            StartCoroutine(SetCanAttack(attack + attackInterval));
            m_CanAttack = false;
        }
        
        IEnumerator DealDamage(float delay)
        {
            yield return new WaitForSeconds(delay);
            DamageEntities();
        }
    }

    public string GetAttack()
    {
        return m_AttackThisFrame;
    }

    public void ResetAttackThisFrame()
    {
        m_AttackThisFrame = "";
    }

    public AiAnimation GetMonsterAnimation()
    {
        return m_MonsterAnimation;
    }

    public void DisableAttack()
    {
        StopCoroutine("SetCanAttack");
        m_CanAttack = false;
    }
    
    private void DamageEntities()
    {
        if (m_AttackSource != null)
        {
            foreach (var source in m_AttackSource)
            {
                var damage = NetworkManager.Instance?.InstantiateMonsterAttack(position: source.position);
            }
        }
    }
    
    bool once = true;
    private void SetNearPlayer()
    {
        float animTime = 0;
        //alerts the animator if the player has entered the vicinity.
        if(once){
            animTime = m_MonsterAnimation.ReactToPlayer();
            once = false; //This fix is flawed but hopefully will minimize bugs.
        }
        StartCoroutine(SetAfterAnim(animTime));
        IEnumerator SetAfterAnim(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_StateMachineAnim.SetBool("nearPlayer", true);
        }
    }

    private void Start()
    {
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;

        m_HealthHandler = GetComponent<HealthHandler>();
        m_MonsterSkin = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_DeathParticles = GetComponentsInChildren<ParticleSystem>();

        if (m_HealthHandler != null)
        {
            m_HealthHandler.HealthChanged += OnHealthChanged;
            m_HealthHandler.HealthDepleted += OnDeath;
        }
        else
        {
            Debug.LogError("No damage system, won't be able to damage players");
        }
    }

    public bool IsDead
    {
        get => m_isDead;
        set => m_isDead = value;
    }

    public void OnDeath()
    {
        m_isDead = true;
        float deathAnimTime = m_MonsterAnimation.Death();
        m_StateMachineAnim.SetBool("dead", true);
        StartCoroutine(DestroyMonster(deathAnimTime));
        enabled = false;
    }

    IEnumerator DestroyMonster(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (SkinnedMeshRenderer s in m_MonsterSkin) {
            s.enabled = false;
        }
        
        if (m_DeathParticles != null)
        {
            foreach (ParticleSystem p in m_DeathParticles)
            {
                if (p.gameObject.name == "DeathVfx")
                {
                    AudioManager.m_Instance.PlaySound("MONSTER_Death", 3.0f, 1.2f);
                    p.Play();
                }
            }
        }
        Destroy(gameObject, 3);
    }

    private void RotateTowardsNearestPlayer()
    {
        if (m_NearestPlayer == null)
        {
            return;
        }
        Vector3 direction = (m_NearestPlayer.position - transform.position).normalized;
        direction.y = 0;
        if (direction.x != 0 && direction.z != 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_Agent.angularSpeed);
        }
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
        if(DEBUG_DEATH)
            OnDeath();
        if (DEBUG_GOTHIT)
        {
            DEBUG_GOTHIT = false;
            m_MonsterAnimation.TakenDamage();
        }

        if (m_isDead)
            return;

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

    //IEnumerator SetNotStun(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //   //Divide by 2 for now
    //    m_IsStun = false;
    //}
    //private IEnumerator stunRoutine;
    private void OnHealthChanged(float deltaHealth)
    {
        if (deltaHealth < 0 && !m_isDead)
        {
            //m_IsStun = true;
            float delay = m_MonsterAnimation.TakenDamage();
            //StopCoroutine(stunRoutine);
            //stunRoutine = SetNotStun(delay);
            //StartCoroutine(stunRoutine);
        }
    }

    private void OnDestroy()
    {
        if (m_HealthHandler != null)
        {
            m_HealthHandler.HealthChanged -= OnHealthChanged;

            if (m_MonsterAnimation != null)
                m_HealthHandler.HealthDepleted -= OnDeath;
        }
    }
}
