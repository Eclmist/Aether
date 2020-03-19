using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarHandler : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealth = 100f;
    [SerializeField]
    private float m_DeltaMultiplier = 5f;
    [SerializeField]
    private float m_CriticalValue = 20f;

    [SerializeField]
    private Image m_HealthBar;

    [SerializeField]
    private Animator m_Animator;

    private float m_HealthDelta = 0.0f;
    private float m_InvMaxHealth = 0.0f;

    void Update()
    {
        // Get inverse of max health if positive
        if (m_MaxHealth > 0.0f)
            m_InvMaxHealth = 1.0f / m_MaxHealth;

        if(m_Animator != null)
        {
            if (IsInCriticalHealth())
                m_Animator.SetBool("isCriticalHealth", true);
            else
                m_Animator.SetBool("isCriticalHealth", false);
        }

        if (m_HealthBar != null && m_HealthDelta >= 0.0f)
            UpdateHealthPercentage();
    }

    public void ModifyHealth(float deltaAmount) 
    {
        m_HealthDelta += deltaAmount * m_InvMaxHealth;
    }

    private void UpdateHealthPercentage()
    {
        // Max delta per frame
        float maxDelta = m_DeltaMultiplier * Time.deltaTime;

        // Update health
        if (m_HealthDelta > maxDelta)
        {
            m_HealthBar.fillAmount += maxDelta;
            m_HealthDelta -= maxDelta;
        }
        else if (m_HealthDelta < -maxDelta)
        {
            m_HealthBar.fillAmount -= maxDelta;
            m_HealthDelta += maxDelta;
        }
        else
        {
            m_HealthBar.fillAmount += m_HealthDelta;
            m_HealthDelta = 0.0f;
        }

        // Clamp health
        m_HealthBar.fillAmount = Mathf.Clamp01(m_HealthBar.fillAmount);
    }

    private bool IsInCriticalHealth()
    {
        return m_HealthBar.fillAmount <= m_CriticalValue * m_InvMaxHealth;
    }
}
