using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/*
 * Class that all states extend for AI
 */
public abstract class AiStateBehaviour : StateMachineBehaviour
{
    protected NavMeshAgent m_Agent;
    protected AiActor m_AiActor;
    protected Animator m_StateAnimator;
    protected Transform m_NearestPlayer;
    protected const float UPDATE_RATE = 0.15f; //Used to circumvent expensive computation every frame.
    private Coroutine m_Update;
    protected void GetReference(Animator animator)
    {
        if (m_Agent == null)
        {
            m_Agent = animator.gameObject.GetComponent<NavMeshAgent>();
        }
        if (m_AiActor == null)
        {
            m_AiActor = animator.gameObject.GetComponent<AiActor>();
            m_NearestPlayer = m_AiActor.m_NearestPlayer;
        }

        m_StateAnimator = animator;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetReference(animator);
        Init();
        m_AiActor.StartCoroutine(IntervalUpdate());
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Update();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_Update != null)
            m_AiActor.StopCoroutine(m_Update);
        OnExit();
    }

    /*
     * All virtual methods used for overriding behaviour.
     */
    public virtual void Init()
    {
    }

    public virtual void Update()
    {
    }
    
    public virtual void CoroutineUpdate()
    {
    }
    
    IEnumerator IntervalUpdate()
    {
        CoroutineUpdate();
        yield return new WaitForSeconds(UPDATE_RATE);
        m_Update = m_AiActor.StartCoroutine(IntervalUpdate());
    }
    
    public virtual void OnExit(){}

    /*
     * An exit state will be called and played.
     */
    protected void ExitState()
    {
        //m_StateAnimator.Play("Exit");
        m_StateAnimator.SetTrigger("Exit"); 
    }
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
