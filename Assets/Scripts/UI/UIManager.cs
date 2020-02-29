using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject m_powerUpHandler;

    [SerializeField]
    private GameObject m_healthBarHandler;

    [SerializeField]
    private GameObject m_skillsUIHandler;

    void Start()
    {
        AetherInput.GetPlayerActions().SwitchSkills.performed += SwitchPlayerSkills;    
    }

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
    
    public void SwitchPlayerSkills(InputAction.CallbackContext ctx)
    {
        if (m_skillsUIHandler != null)
        {
            SkillsUIHandler handler = m_skillsUIHandler.GetComponent<SkillsUIHandler>();
            if (handler != null)
            {
                handler.SwitchSkills();
                handler.SwitchIcons();
            }   
        }
    }
}
