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
        AetherInput.GetPlayerActions().SwitchSkills.performed += SwitchPlayerSkills;    
    }

    public void ActivatePowerupIcon(UIPowerUpSignals signal)
    {
        if (m_powerUpHandler != null)
        {
            if (signal != null) 
                m_powerUpHandler.ActivateIcon(signal);

        }
    }

    public void ModifyHealthBar(float percentageChange)
    {
        if (m_healthBarHandler != null)
             m_healthBarHandler.IndicateDamage(percentageChange);
                
    }

    // To Be Refactored
    
    public void SwitchPlayerSkills(InputAction.CallbackContext ctx)
    {
        if (m_skillsUIHandler != null)
        {
            m_skillsUIHandler.SwitchSkillsSprites(); 
        }
    }

    public void SaveSkill(ItemSkill itemSkill) 
    {
        m_skillsUIHandler.HandleSkillPickUp(itemSkill);
    }

    public int GetSkillsIndex()
    {
        return m_skillsUIHandler.GetSkillsIndex();
    }

    public void RemoveSkill()
    {
         m_skillsUIHandler.RemoveUsedSkill();
    }
}
