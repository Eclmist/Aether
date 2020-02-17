using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class AiMonster : AiActor, Attacker
{
    [SerializeField] private Animator m_PlantMonsterAnimator;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            BreakIdle();
        }
    }

    bool canAttack = true;
    public void Attack(float attackInterval)
    {
        if (canAttack)
        {
            //logic for damaging the player here
            m_PlantMonsterAnimator.SetTrigger("attack1");
            StartCoroutine(setCanAttack());
            canAttack = false;
        }

        IEnumerator setCanAttack()
        {
            yield return new WaitForSeconds(attackInterval);
            canAttack = true;
        }
    }
    

    private void BreakIdle()
    {
        //alerts the animator if the player has entered the vicinity.
        m_StateMachineAnim.SetBool("isSafe", false);
        m_PlantMonsterAnimator.SetTrigger("GoAlive");
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

}