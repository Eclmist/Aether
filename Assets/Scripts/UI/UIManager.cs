using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Animator m_NotificationAnimator;
    [SerializeField]
    private Text m_NotificationText;

    [SerializeField]
    private Animator m_CountdownAnimator;
    
    [SerializeField]
    private Text m_CountdownText;

    [SerializeField]
    private UIPowerUpHandler m_UIPowerUpHandler;
    [SerializeField]
    private UISkillsHandler m_UISkillsHandler;

    private void Awake()
    {
        GameManager.Instance.GameStarted += OnGameStarted;
    }

    private void Update()
    {
        // E4 hack
        // Look for a monster and toggle their health UI
        Ray screenRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(screenRay, out hit, 20))
        {
            hit.collider.GetComponent<UIDisplayOnHover>()?.OnHover();
        }
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

        StartCoroutine(UIStartGame(message));
    }

    public IEnumerator UIStartGame(string message)
    {
        UINotifyHeader(message);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(CountdownActivate());
    }

    private IEnumerator CountdownActivate()
    {
        int counter = 3;

        while (counter > 0) 
        {
            if (m_CountdownText != null)
                m_CountdownText.text = counter + "";
            if (m_CountdownAnimator != null)
                m_CountdownAnimator.SetTrigger("Open");

            counter--;

            yield return new WaitForSeconds(1.5f);
        }

        if (m_CountdownText != null)
            m_CountdownText.text = "Go!";
        if (m_CountdownAnimator != null)
            m_CountdownAnimator.SetTrigger("Open");

        GameManager.Instance.SetUnFrozen();
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

    // To Be Refactored

    public void SwitchPlayerSkills()
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.SwitchSpriteIcons();
    }

    public void SaveSkill(SkillItem skillItem)
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.HandleSkillPickUp(skillItem);
    }

    public void RemoveSkill()
    {
        if (m_UISkillsHandler != null)
            m_UISkillsHandler.RemoveUsedSkill();
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.GameStarted -= OnGameStarted;
    }

    public void HideAllHUD()
    {
        // TODO: ROBY PLEASE FIX. DON'T USE FIND.
        GameObject gameHUD = GameObject.Find("HUD_Updated");
        if (gameHUD != null)
        {
            gameHUD.SetActive(false);
        }
    }

    public void ShowWinningMessage(Team team)
    {
        HideAllHUD();
        // Show winning message
    }
}
