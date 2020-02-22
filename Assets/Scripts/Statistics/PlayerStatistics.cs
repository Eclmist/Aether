using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    private Health m_health;

    // Start is called before the first frame update
    void Start()
    {
        m_health = GetComponent<Health>();
    }

    public bool IsDead()
    {
        return m_health.IsZero();
    }

    public void UpdateHealth(float change)
    {
        if (IsDead())
            return;

        if (change <= 0.0f)
        {
            m_health.Damage(-1.0f * change);
        } 
        else {
            m_health.Heal(change);
        }
    }
}
