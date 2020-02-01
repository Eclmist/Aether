using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Transform player;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //alerts the animator if the player has entered the vicinity.
            animator.SetBool("isSafe", false);
        }
    }
    
}
