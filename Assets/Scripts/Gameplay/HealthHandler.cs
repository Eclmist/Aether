using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    private const float MAX_HEALTH = 100f;

    public event System.Action<float> HealthChanged;
    public event System.Action HealthDepleted;

    [SerializeField]
    private float m_Health = MAX_HEALTH;

    private bool m_IsDead = false;

    public void Damage(float amount)
    {
        if (m_IsDead)
            return;

        float newHealth = Mathf.Max(0.0f, m_Health - amount);
        HealthChanged?.Invoke((m_Health - newHealth) / MAX_HEALTH);
        m_Health = newHealth;

        if (m_Health == 0.0f)
            HealthDepleted?.Invoke();
    }
}
