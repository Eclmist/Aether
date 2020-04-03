using System.Collections;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public event System.Action<float> HealthChanged;
    public event System.Action HealthDepleted;

    [SerializeField]
    private float m_MaxHealth = 100f;

    [SerializeField]
    private float m_Health;

    private bool m_DamagedInCurrentFrame = false;
    private bool m_DeadInCurrentFrame = false;
    private bool m_IsDead = false;

    public void Start()
    {
        m_Health = m_MaxHealth;
    }

    public float GetHealth()
    {
        return m_Health;
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    public float GetPercentageHealth()
    {
        return m_Health / m_MaxHealth;
    }

    public void Damage(float amount)
    {
        if (m_IsDead)
            return;

        float newHealth = Mathf.Max(0.0f, m_Health - amount);
        HealthChanged?.Invoke(newHealth - m_Health);

        m_Health = newHealth;
        StartCoroutine(SetDamaged());

        if (m_Health == 0.0f)
        {
            HealthDepleted?.Invoke();
            StartCoroutine(SetDead());
        }
    }

    public void Revive()
    {
        m_Health = m_MaxHealth;
        m_IsDead = false;
    }

    public bool DamagedInCurrentFrame()
    {
        return m_DamagedInCurrentFrame;
    }

    private IEnumerator SetDamaged()
    {
        m_DamagedInCurrentFrame = true;
        yield return new WaitForEndOfFrame();
        m_DamagedInCurrentFrame = false;
    }

    public bool DeadInCurrentFrame()
    {
        return m_DeadInCurrentFrame;
    }

    private IEnumerator SetDead()
    {
        m_IsDead = true;
        m_DeadInCurrentFrame = true;
        yield return new WaitForEndOfFrame();
        m_DeadInCurrentFrame = false;
    }
}
