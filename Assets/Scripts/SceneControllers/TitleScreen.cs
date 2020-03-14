using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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

    private EventSystem m_EventSystem;

    [SerializeField]
    private GameObject m_MainMultiplayerButton;

    [SerializeField]
    private GameObject m_SideMultiplayerButton;

    void Start() 
    {
        m_isMultiplayerDropdownActivated = false;
        m_EventSystem = EventSystem.current;
        AetherInput.GetUIActions().Cancel.performed += SwitchMultiplayMenuBarsCallbacl;

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

    public void SwitchMultiplayMenuBarsCallbacl(InputAction.CallbackContext ctx)
    {
        SwitchMultiplayerMenuBars();
    }

    public void SwitchMultiplayerMenuBars()
    {
        AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);

        if (m_EventSystem == null) 
            return;

        if (m_MainMultiplayerButton == null || m_SideMultiplayerButton == null)
            return;

        if (m_isMultiplayerDropdownActivated) 
        {
            m_EventSystem.SetSelectedGameObject(m_MainMultiplayerButton);
            m_MainCanvasAnimator.SetTrigger("ReverseMultiplayer");
        } 
        else {
            m_EventSystem.SetSelectedGameObject(m_SideMultiplayerButton);
            m_MainCanvasAnimator.SetTrigger("EnterMultiplayer");
        }

        m_isMultiplayerDropdownActivated = !(m_isMultiplayerDropdownActivated);
    }

    public void GoToCharacterCustomization()
    {
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
