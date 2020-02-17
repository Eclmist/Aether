using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/*
 * Class that all states extend for AI
 */
public class AiState : StateMachineBehaviour
{
    private NavMeshAgent m_agent;

    private AiActor m_aiActor;

    public float distanceFlee, safeDistance, speedBoost;
    private float runningSpeed; 
    private Boolean isDelay;
    
    private void GetReference(Animator animator)
    {
        if (m_agent == null)
        {
            m_agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
        if (m_aiActor == null)
        {
            m_aiActor = animator.gameObject.GetComponent<AiActor>();
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, Int32 layerIndex)
    {
        GetReference(animator);
        runningSpeed = m_agent.speed + speedBoost;
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 aiLocation = animator.transform.position;
        Vector3 playerLocation = m_aiActor.player.position;
        float distanceBetween = Vector3.Distance(playerLocation, aiLocation);
        
        
        if (distanceFlee > distanceBetween && !isDelay)
        {
            RunAway(playerLocation, aiLocation);
        }
        else if (distanceBetween > safeDistance)
        {
            m_agent.speed = runningSpeed - speedBoost;
            //terminates this state
            animator.SetBool("isSafe", true);
        }
    }

    private void RunAway(Vector3 playerLocation, Vector3 aiLocation)
    {
        m_agent.speed = runningSpeed;
        m_agent.SetDestination(aiLocation + aiLocation - playerLocation);
        m_aiActor.StartCoroutine(SetDelay());
    }
    
    IEnumerator SetDelay()
    {
        isDelay = true;
        yield return new WaitForSeconds(0.2f);
        isDelay = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
