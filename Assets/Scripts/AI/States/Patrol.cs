using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Patrol : StateMachineBehaviour
{
    [SerializeField]
    public Transform[] waypoints;
    private NavMeshAgent agent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckForAgent(animator);
        GotoNextPoint();
    }

    private void CheckForAgent(Animator animator)
    {
        if (agent == null)
        {
            agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
    }
    

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckForAgent(animator);
        
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
            GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        Transform destination = waypoints[Random.Range(0, waypoints.Length)];
        Debug.Log(destination.position);
        agent.SetDestination(destination.position);
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
