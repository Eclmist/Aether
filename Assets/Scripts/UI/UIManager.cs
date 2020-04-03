using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    // Hacks pls help me fix lol, i need to call UIManager from another class (Game manager)
    public static UIManager uiManager;

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
    [SerializeField]
    private GameObject m_WinningMessageTeamA;
    [SerializeField]
    private GameObject m_WinningMessageTeamB;

    private void Awake()
    {
        GameManager.Instance.GameStarted += OnGameStarted;
        uiManager = this;
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
        // Attach Player Health to Health Bar
        m_UIHealthBarHandler.SetPlayerAttachment(PlayerManager.Instance.GetLocalPlayer());

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
        GameObject gameHUD = GameObject.Find("HUD_Updated");
        if (gameHUD != null)
        {
            gameHUD.SetActive(false);
        }
    }

    public void ShowWinningMessageTeamA()
    {
        HideAllHUD();
        m_WinningMessageTeamA.SetActive(true);
    }

    public void ShowWinningMessageTeamB()
    {
        HideAllHUD();
        m_WinningMessageTeamB.SetActive(true);
    }
}
