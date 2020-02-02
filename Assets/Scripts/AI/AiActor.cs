using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AiActor : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    private Animator m_StateAnimator;
    public Transform player;

    public void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_StateAnimator = GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //alerts the animator if the player has entered the vicinity.
            m_StateAnimator.SetBool("isSafe", false);
        }
    }

    public void SetInactive()
    {
        m_Agent.enabled = false;
        m_StateAnimator.enabled = false;
    }
    
    public void SetActive()
    {
        m_Agent.enabled = true;
        m_StateAnimator.enabled = true;
    }
    
}
