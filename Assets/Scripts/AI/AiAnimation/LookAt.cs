using UnityEngine;

/*
 * Used to control the look at position for a m_head transform
 */
[RequireComponent (typeof (Animator))]
public class LookAt : MonoBehaviour {
    [SerializeField]
    private Transform m_head = null;
    [SerializeField]
    private Vector3 lookAtTargetPosition;
    [SerializeField]
    private float lookAtCoolTime = 0.2f;
    [SerializeField]
    private float lookAtHeatTime = 0.2f;
    [SerializeField]
    private bool m_isLooking = true;
    
    private Vector3 lookAtPosition;
    private Animator animator;
    private float lookAtWeight = 0.0f;

    void Start ()
    {
        if (!m_head)
        {
            Debug.LogError("No m_head transform - LookAt disabled");
            enabled = false;
            return;
        }
        animator = GetComponent<Animator>();
        lookAtTargetPosition = m_head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;
    }

    void OnAnimatorIK ()
    {
        lookAtTargetPosition.y = m_head.position.y;
        float lookAtTargetWeight = m_isLooking ? 1.0f : 0.0f;

        Vector3 curDir = lookAtPosition - m_head.position;
        Vector3 futDir = lookAtTargetPosition - m_head.position;

        curDir = Vector3.RotateTowards(curDir, futDir, 6.28f*Time.deltaTime, float.PositiveInfinity);
        lookAtPosition = m_head.position + curDir;

        float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
        lookAtWeight = Mathf.MoveTowards (lookAtWeight, lookAtTargetWeight, Time.deltaTime/blendTime);
        animator.SetLookAtWeight (lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        animator.SetLookAtPosition (lookAtPosition);
    }
}
