using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AiActor : MonoBehaviour
{
    protected NavMeshAgent m_Agent;
    protected Animator m_StateMachineAnim;
    public Transform m_Player;

    public void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_StateMachineAnim = GetComponent<Animator>();
    }
    
    public void SetInactive()
    {
        m_Agent.enabled = false;
        m_StateMachineAnim.enabled = false;
    }
    
    public void SetActive()
    {
        m_Agent.enabled = true;
        m_StateMachineAnim.enabled = true;
    }
    
}
