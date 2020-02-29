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

        float axisDeltaMagnitude = m_Player.networkObject.axisDeltaMagnitude;
        transform.rotation = m_Player.networkObject.rotation;
        m_Animator.SetFloat("Velocity-XZ-Normalized-01", axisDeltaMagnitude);
        m_Animator.SetFloat("Velocity-Y-Normalized", m_Player.networkObject.vertVelocity);
        m_Animator.SetBool("Grounded", m_Player.networkObject.grounded);
    }

    public void TriggerJump()
    {
        m_Animator.SetTrigger("Jump");
    }
}
