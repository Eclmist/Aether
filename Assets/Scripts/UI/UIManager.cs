using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Animator m_NotificationAnimator;
    [SerializeField]
    private Text m_NotificationText;

    [SerializeField]
    private UIPowerUpHandler m_UIPowerUpHandler;
    [SerializeField]
    private UISkillsHandler m_UISkillsHandler;
    [SerializeField]
    private UIHealthBarHandler m_UIHealthBarHandler;

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
            m_UIPowerUpHandler.ActivateIcon(signal);
    }

    public void Damage(float damageAmount)
    {
        if (m_UIHealthBarHandler != null)
             m_UIHealthBarHandler.ModifyHealth(-damageAmount);
    }

    public void Heal(float healAmount)
    {
        if (m_UIHealthBarHandler != null)
            m_UIHealthBarHandler.ModifyHealth(healAmount);
    }

    // To Be Refactored

    public void SwitchPlayerSkills()
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.SwitchSpriteIcons();
    }

    public void SaveSkill(ItemSkill itemSkill)
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.HandleSkillPickUp(itemSkill);
    }

    public void RemoveSkill()
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.RemoveUsedSkill();
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameStarted -= OnGameStarted;
    }
}
