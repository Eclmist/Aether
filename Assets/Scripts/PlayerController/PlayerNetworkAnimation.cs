using UnityEngine;

[RequireComponent(typeof(PlayerNetworkHandler))]
public class PlayerNetworkAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;

    private PlayerNetworkHandler m_PlayerNetworkHandler;

    void Start()
    {
        m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
    }

    void Update()
    {
        if (m_PlayerNetworkHandler.networkObject == null)
            return;

        float axisMagnitude = m_PlayerNetworkHandler.networkObject.axisMagnitude;
        transform.rotation = m_PlayerNetworkHandler.networkObject.rotation;
        m_Animator.SetFloat("MovementInput", axisMagnitude);
        m_Animator.SetFloat("VerticalVelocity", m_PlayerNetworkHandler.networkObject.vertVelocity);
        m_Animator.SetBool("Grounded", m_PlayerNetworkHandler.networkObject.grounded);

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (axisMagnitude > 0.01f)
                m_Animator.speed = m_AnimationSpeedCurve.Evaluate(axisMagnitude); // Make running animation speed match actual movement speed
            else
                m_Animator.speed = 1;
        }
    }
}
