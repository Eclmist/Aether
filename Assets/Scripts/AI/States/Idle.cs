using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/*
 * Requires "isSafe" parameter in the animator. Will flee from player when nearby.
 */
public class Idle : AiStateBehaviour
{
    public override void Init()
    { 
        m_AiActor.SetInactive();
    }

    public override void Exit()
    {
        m_AiActor.SetActive();
    }

}
