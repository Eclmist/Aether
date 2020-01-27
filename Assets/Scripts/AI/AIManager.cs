using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{
    public GameObject[] waypoints;
    private NavMeshAgent agent;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public GameObject[] Waypoints
    {
        get => waypoints;
        set => waypoints = value;
    }

    
}
