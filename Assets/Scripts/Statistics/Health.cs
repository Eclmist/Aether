using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float m_healthPercentage;

    public bool IsZero()
    {
        return m_healthPercentage <= 0.0f;
    }

    public void Damage(float change)
    {
        UIManager.Instance.ModifyHealthBar(change);
        if (m_healthPercentage <= change)
        {
            m_healthPercentage = 0.0f;
            return;
        }

        m_healthPercentage -= change;
    }

    public void Heal(float change)
    {
        if (m_healthPercentage + change >= 1.0f)
        {
            m_healthPercentage = 1.0f;
            return;
        }

        m_healthPercentage += change;
    }
}
