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
    [HideInInspector]
    public Transform m_NearestPlayer; 
    protected Vector3 m_SpawnPos;

    public void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_StateMachineAnim = GetComponent<Animator>();
        m_SpawnPos = transform.position;
        m_NearestPlayer = null;
    }

    public Vector3 GetSpawnPos()
    {
        return m_SpawnPos;
    }

    public virtual void SetInactive()
    {
        enabled = false;
    }
    
    public virtual void SetActive()
    {
        enabled = true;
        m_StateMachineAnim.enabled = true;
    }

    public virtual float DistanceFromSpawnPoint()
    {
        return Vector3.Distance(transform.position, m_SpawnPos);
    }
    
}
