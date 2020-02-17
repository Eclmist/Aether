using UnityEngine;
using UnityEngine.AI;

/*
 * Class that all states extend for AI
 */
public abstract class AiStateBehaviour : StateMachineBehaviour
{
    protected NavMeshAgent m_Agent;
    protected AiActor m_AiActor;
    protected Transform m_NearestPlayer;
    protected void GetReference(Animator animator)
    {
        if (m_Agent == null)
        {
            m_Agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
        if (m_AiActor == null)
        {
            m_AiActor = animator.gameObject.GetComponent<AiActor>();
            m_NearestPlayer = m_AiActor.m_Player;
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetReference(animator);
        Init();
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Update();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Exit();
    }

    public virtual void Init()
    {
    }

    public virtual void Update()
    {
    }
    public virtual void Exit(){}
    

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
