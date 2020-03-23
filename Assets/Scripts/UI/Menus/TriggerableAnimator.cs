using UnityEngine;
using UnityEngine.UI;

public class TriggerableAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    private bool m_Trigger;

    [SerializeField]
    private string m_OnTrigger;

    [SerializeField]
    private string m_OffTrigger;

    public void TriggerAnimation()
    {
        if (m_Animator == null)
            return;

        if (m_Trigger)
        {
            AudioManager.m_Instance.PlaySound("GEN_Success_2", 1.0f, 1.0f);
            m_Animator.SetTrigger(m_OffTrigger);
        }
        else 
        {
            AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
            m_Animator.SetTrigger(m_OnTrigger);
        }

        m_Trigger = !(m_Trigger);
    }

    public bool GetTriggerBool()
    {
        return m_Trigger;
    }
}
