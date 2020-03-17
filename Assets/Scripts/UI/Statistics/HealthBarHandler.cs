using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField]
    private Image m_healthDecrement;

    [SerializeField]
    private Animator m_Animator;

    private float m_changeAmount;

    private const float m_decrementMultiplier = 0.05f;

    private const float m_criticalLimit = 0.2f;

    void Start()
    {
        m_changeAmount = 0.0f;
    }

    void Update()
    {
        if(m_Animator != null)
        {
            if (IsInCriticalHealth())
            {
                m_Animator.SetBool("isCriticalHealth", true);
            }
            else {
                 m_Animator.SetBool("isCriticalHealth", false);
            }
        }

        if (m_healthDecrement != null && m_changeAmount >= 0.0f)
            UpdateHealthPercentage(Time.deltaTime);
            
    }
    public void IndicateDamage(float percentageChange) 
    {
        m_changeAmount += percentageChange;
    }

    private void UpdateHealthPercentage(float seconds)
    {
        float decrement = m_decrementMultiplier * seconds;

        // Consider edge case when you have nothing to deplete.
        if (m_healthDecrement.fillAmount <= decrement)
        {
            m_healthDecrement.fillAmount = 0.0f;
            m_changeAmount = 0.0f;
            return;
        }

        if (decrement > m_changeAmount)
        {
            m_healthDecrement.fillAmount -= m_changeAmount;
            m_changeAmount = 0.0f;
        }
        else {
            m_healthDecrement.fillAmount -= decrement;
            m_changeAmount -= decrement;
        }
    }

    private bool IsInCriticalHealth()
    {
        return m_healthDecrement.fillAmount <= m_criticalLimit;
    }
}
