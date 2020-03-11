using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private PowerUpUIHandler m_powerUpHandler;

    [SerializeField]
    private HealthBarHandler m_healthBarHandler;

    [SerializeField]
    private SkillsUIHandler m_skillsUIHandler;

    void Start()
    {
  
    }

    public void ActivatePowerupIcon(UIPowerUpSignals signal)
    {
        if (m_powerUpHandler != null)
        {
            m_powerUpHandler.ActivateIcon(signal);
        }
    }

    public void ModifyHealthBar(float percentageChange)
    {
        if (m_healthBarHandler != null)
             m_healthBarHandler.IndicateDamage(percentageChange);
                
    }

    // To Be Refactored
    
    public void SwitchPlayerSkills()
    {
        if (m_skillsUIHandler != null)
        {
            m_skillsUIHandler.SwitchSpriteIcons(); 
        }
    }

    public void SaveSkill(ItemSkill itemSkill) 
    {
        m_skillsUIHandler.HandleSkillPickUp(itemSkill);
    }

    public void RemoveSkill()
    {
         m_skillsUIHandler.RemoveUsedSkill();
    }

}
