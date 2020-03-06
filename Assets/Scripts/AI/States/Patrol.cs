using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/*
 * TODO: change this to Ai state behaviour
 * Patrols around waypoints
 */
public class Patrol : StateMachineBehaviour
{
    [FormerlySerializedAs("waypoints")] [SerializeField]
    private Transform[] m_waypoints;
    private NavMeshAgent m_agent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckForAgent(animator);
        GotoNextPoint();
    }

    private void CheckForAgent(Animator animator)
    {
        if (m_agent == null)
        {
            m_agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
    }
    

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckForAgent(animator);
        
        if (!m_agent.pathPending && m_agent.remainingDistance < m_agent.stoppingDistance)
            GotoNextPoint();
    }

    private void GotoNextPoint()
    {
        Transform destination = m_waypoints[Random.Range(0, m_waypoints.Length)];
        m_agent.SetDestination(destination.position);
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
