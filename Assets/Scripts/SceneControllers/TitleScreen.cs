using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private Animator m_MainCanvasAnimator;

    [SerializeField]
    private GameObject[] m_DebugObjects;

    [SerializeField]
    private Animator m_ScreenFadeAnimator;

    [SerializeField]
    private MultiplayerMenu m_ForgeMultiplayerMenu;

    private bool m_isMultiplayerDropdownActivated;

    private bool m_AnyKeyPressed;

    private bool m_IsLoading;

    private EventSystem m_EventSystem;

    [SerializeField]
    private GameObject m_MainMultiplayerButton;

    [SerializeField]
    private GameObject m_SideMultiplayerButton;

    [SerializeField]

    private GameObject m_OptionsConnectButton;

    void Start() 
    {
        m_EventSystem = EventSystem.current;
        AetherInput.GetUIActions().Cancel.performed += SwitchMultiplayMenuBarsCallback;
        AetherInput.GetUIActions().Cancel.performed += SwitchOptionsMenuCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && m_MainCanvasAnimator != null && !m_AnyKeyPressed)
        {
            // Ignore mouse input 
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;

            m_MainCanvasAnimator.SetTrigger("Start");
            AudioManager.m_Instance.PlaySound("GEN_Success_2", 1.0f, 1.0f);
            m_AnyKeyPressed = true;
        }

        // TODO: Implement cheat manager
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (GameObject target in m_DebugObjects)
            {
                target.SetActive(true);
            }
        }
    }

    public void SwitchOptionsMenuCallback(InputAction.CallbackContext ctx)
    {
        bool triggerBoolean = MenusManager.Instance.GetTriggerBool(0);
        if (triggerBoolean)
        {
            TriggerOptionsAnimator();
        }
    }

    public void SwitchMultiplayMenuBarsCallback(InputAction.CallbackContext ctx)
    {
        if (m_isMultiplayerDropdownActivated)
            SwitchMultiplayerMenuBars();
    }

    public void TriggerOptionsAnimator()
    {
        if (m_IsLoading)
            return;

        bool triggerBoolean = MenusManager.Instance.GetTriggerBool(0);

        if (m_OptionsConnectButton == null || m_SideMultiplayerButton == null )
            return;

        MenusManager.Instance.TriggerAnimator(0);

        if (triggerBoolean)
        {
             StartCoroutine(DelaySelection(m_SideMultiplayerButton));
        } 
        else {
             m_EventSystem.SetSelectedGameObject(m_OptionsConnectButton);
        }
    }

    private IEnumerator DelaySelection(GameObject button)
    {   
        yield return new WaitForSeconds(1.0f);
         m_EventSystem.SetSelectedGameObject(button);
    }

    public void SwitchMultiplayerMenuBars()
    {
        if (m_IsLoading)
            return;

        if (MenusManager.Instance.GetTriggerBool(0))
            return;

        if (m_EventSystem == null) 
            return;

        if (m_MainMultiplayerButton == null || m_SideMultiplayerButton == null)
            return;

        if (m_isMultiplayerDropdownActivated) 
        {
            AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
            m_EventSystem.SetSelectedGameObject(m_MainMultiplayerButton);
            m_MainCanvasAnimator.SetTrigger("ReverseMultiplayer");
        } 
        else {
            AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
            m_EventSystem.SetSelectedGameObject(m_SideMultiplayerButton);
            m_MainCanvasAnimator.SetTrigger("EnterMultiplayer");
        }

        m_isMultiplayerDropdownActivated = !(m_isMultiplayerDropdownActivated);
    }

    public void GoToCharacterCustomization()
    {
        if (m_IsLoading)
            return;

        m_IsLoading = true;
        AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
        m_ScreenFadeAnimator.SetTrigger("ToBlack");
        StartCoroutine("HostLobby");
    }

    private IEnumerator HostLobby()
    {
        yield return new WaitForSeconds(1.5f);
        m_ForgeMultiplayerMenu.Host();
    }

    public void LoadVisibilityGym()
    {
        SceneManager.LoadScene("GYM_Visibility 1");
        AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
