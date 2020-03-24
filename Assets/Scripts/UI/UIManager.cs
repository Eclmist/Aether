using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private PowerUpUIHandler m_powerUpHandler;

    [SerializeField]
    private HealthBarHandler m_healthBarHandler;

    [SerializeField]
    private SkillsUIHandler m_skillsUIHandler;

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

    public void SaveSkill(SkillItem skillItem) 
    {
        m_skillsUIHandler.HandleSkillPickUp(skillItem);
    }

    public void RemoveSkill()
    {
         m_skillsUIHandler.RemoveUsedSkill();
    }

}
