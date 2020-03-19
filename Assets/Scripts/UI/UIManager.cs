using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Animator m_NotificationAnimator;
    [SerializeField]
    private Text m_NotificationText;

    [SerializeField]
    private UIPowerUpHandler m_UIPowerUpHandler;
    [SerializeField]
    private SkillsUIHandler m_SkillsUIHandler;
    
    [SerializeField]
    private HealthBarHandler m_HealthBarHandler;

    private void Awake()
    {
        GameManager.Instance.GameStarted += OnGameStarted;
    }

    private void OnGameStarted(GameMode gameMode)
    {
        string message = "";
        switch (gameMode)
        {
            case GameMode.GAMEMODE_KING_OF_THE_HILL:
                message = "King of the Hill";
                break;
            default:
                break;
        }

        UINotifyHeader(message);
    }

    public void UINotifyHeader(string message)
    {
        if (m_NotificationText != null)
            m_NotificationText.text = message;
        if (m_NotificationAnimator != null)
            m_NotificationAnimator.SetTrigger("Open");
    }

    public void UINotifyPanel(string message)
    {
        // push new notifications to side panel
    }

    public void ActivatePowerupIcon(UIPowerUpSignals signal)
    {
        if (m_UIPowerUpHandler != null)
        {
            m_UIPowerUpHandler.ActivateIcon(signal);
        }
    }

    public void ModifyHealthBar(float percentageChange)
    {
        if (m_HealthBarHandler != null)
             m_HealthBarHandler.IndicateDamage(percentageChange);
    }

    // To Be Refactored
    
    public void SwitchPlayerSkills()
    {
        if (m_SkillsUIHandler != null)
        {
            m_SkillsUIHandler.SwitchSpriteIcons(); 
        }
    }

    public void SaveSkill(ItemSkill itemSkill) 
    {
        m_SkillsUIHandler.HandleSkillPickUp(itemSkill);
    }

    public void RemoveSkill()
    {
         m_SkillsUIHandler.RemoveUsedSkill();
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameStarted -= OnGameStarted;
    }
}
