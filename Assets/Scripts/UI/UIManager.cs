using System.Collections;
using System.Collections.Generic;
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
        m_UIPowerUpHandler.ActivateIcon(signal);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameStarted -= OnGameStarted;
    }
}
