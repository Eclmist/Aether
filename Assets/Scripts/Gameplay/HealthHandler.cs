using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    private const float MAX_HEALTH = 100f;

    public event System.Action<float> HealthChanged;
    public event System.Action HealthDepleted;

    [SerializeField]
    private float m_Health = MAX_HEALTH;

    public void Damage(float amount)
    {
        float newHealth = Mathf.Max(0.0f, m_Health - amount);
        if (newHealth != m_Health)
        {
            HealthChanged?.Invoke((m_Health - newHealth) / MAX_HEALTH);
            m_Health = newHealth;
        }

        // Trigger animation events here perhaps for getting hit
    }
}
