using System.Reflection;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField]
    protected const float m_BuffDuration = 5.0f;

    protected float m_TimeOfActivation = -1.0f;

    void Update()
    {
        if (m_TimeOfActivation != -1.0f && Time.time > m_TimeOfActivation + m_BuffDuration)
        {
            OnPowerUpExpired();
            Destroy(this);
        }
    }

    public void InitializePowerUp()
    {
        m_TimeOfActivation = Time.time;
        OnPowerUpActivated();
    }
    
    public abstract void OnPowerUpActivated();

    public abstract void OnPowerUpExpired();
}
