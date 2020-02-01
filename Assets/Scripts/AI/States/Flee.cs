using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/*
 * Requires "isSafe" parameter in the animator. Will flee from player when nearby.
 */
public class Flee : StateMachineBehaviour
{
    private NavMeshAgent agent;

    private AiManager aiManager;

    public float distanceFlee, safeDistance, speedBoost;
    private float runningSpeed; 
    private Boolean isDelay;
    
    private void GetReference(Animator animator)
    {
        if (agent == null)
        {
            agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
        if (aiManager == null)
        {
            aiManager = animator.gameObject.GetComponent<AiManager>();
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, Int32 layerIndex)
    {
        GetReference(animator);
        runningSpeed = agent.speed + speedBoost;
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 aiLocation = animator.transform.position;
        Vector3 playerLocation = aiManager.player.position;
        float distanceBetween = Vector3.Distance(playerLocation, aiLocation);
        
        
        if (distanceFlee > distanceBetween && !isDelay)
        {
            Debug.Log(distanceBetween);
            RunAway(playerLocation, aiLocation);
        }
        else if (distanceBetween > safeDistance)
        {
            agent.speed = runningSpeed - speedBoost;
            //terminates this state
            animator.SetBool("isSafe", true);
        }
    }

    private void RunAway(Vector3 playerLocation, Vector3 aiLocation)
    {
        agent.speed = runningSpeed;
        agent.SetDestination(aiLocation + aiLocation - playerLocation);
        aiManager.StartCoroutine(SetDelay());
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
