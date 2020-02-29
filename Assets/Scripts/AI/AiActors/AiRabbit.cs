using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(Animator))]
public class AiRabbit : AiActor
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //alerts the animator if the player has entered the vicinity.
            m_NearestPlayer = other.transform;
            m_StateMachineAnim.SetBool("isSafe", false);
        }
    }
}