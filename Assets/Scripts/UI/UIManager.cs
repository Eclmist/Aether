using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject m_powerUpHandler;

    [SerializeField]
    private GameObject m_healthBarHandler;

    [SerializedField]
    private GameObject m_skillsUIHandler;

    public void ActivatePowerupIcon(UIPowerUpSignals signal)
    {
        if (m_powerUpHandler != null)
        {
            PowerUpUIHandler handler = m_powerUpHandler.GetComponent<PowerUpUIHandler>();
            if (handler != null && signal != null) 
                handler.ActivateIcon(signal);

        }
    }

    public void ModifyHealthBar(float percentageChange)
    {
        if (m_healthBarHandler != null)
        {
            HealthBarHandler handler = m_healthBarHandler.GetComponent<HealthBarHandler>();
            if (handler != null) 
                handler.IndicateDamage(percentageChange);
                
        }
    }
    
    public void SwitchPlayerSkills()
    {
        if (m_skillsUIHandler != null)
        {
            SkillsUIHandler handler = m_skillsUIHandler.GetComponent<SkillsUIHandler>();
            if (handler != null)
                handler.SwitchSkills();
        }
    }
}
