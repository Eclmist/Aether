using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public event System.Action<float> HealthChanged;
    public event System.Action HealthDepleted;

    public const float MAX_HEALTH = 100f;

    [SerializeField]
    private float m_Health = MAX_HEALTH;

    public void Reduce(float amount)
    {
        float newHealth = Mathf.Max(0.0f, m_Health - amount);
        if (newHealth != m_Health)
        {
            HealthChanged?.Invoke((m_Health - newHealth) / MAX_HEALTH);
            m_Health = newHealth;
        }
    }
}
