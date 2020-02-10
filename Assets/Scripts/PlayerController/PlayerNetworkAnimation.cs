using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerNetworkAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;

    private Player m_Player;

    void Start()
    {
        m_Player = GetComponent<Player>();
    }

    void Update()
    {
        if (m_Player.networkObject == null)
            return;

        float axisMagnitude = m_Player.networkObject.axisMagnitude;
        transform.rotation = m_Player.networkObject.rotation;
        m_Animator.SetFloat("MovementInput", axisMagnitude);
        m_Animator.SetFloat("VerticalVelocity", m_Player.networkObject.vertVelocity);
        m_Animator.SetBool("Grounded", m_Player.networkObject.grounded);

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (axisMagnitude > 0.01f)
                m_Animator.speed = m_AnimationSpeedCurve.Evaluate(axisMagnitude); // Make running animation speed match actual movement speed
            else
                m_Animator.speed = 1;
        }
    }
}
