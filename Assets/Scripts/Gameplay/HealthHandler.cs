using System.Collections;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public const float MAX_HEALTH = 100f;

    public event System.Action<float> HealthChanged;
    public event System.Action HealthDepleted;

    [SerializeField]
    private float m_Health = MAX_HEALTH;

    private bool m_DamagedInCurrentFrame = false;
    private bool m_DeadInCurrentFrame = false;
    private bool m_IsDead = false;

    public void Damage(float amount)
    {
        if (m_IsDead)
            return;

        float newHealth = Mathf.Max(0.0f, m_Health - amount);
        HealthChanged?.Invoke(m_Health - newHealth);

        m_Health = newHealth;
        StartCoroutine(SetDamaged());

        if (m_Health == 0.0f)
        {
            HealthDepleted?.Invoke();
            StartCoroutine(SetDead());
        }
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
