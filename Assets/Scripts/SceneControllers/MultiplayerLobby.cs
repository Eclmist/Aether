using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MultiplayerLobby : MonoBehaviour
{
    [SerializeField]
    private Animator m_UIAnimator;

    [SerializeField]
    private Animator m_ScreenFadeAnimator;

    [SerializeField]
    private Animator m_FooterAnimator;

    [SerializeField]
    private LobbySystem m_LobbySystem;

    private bool m_IsInCustomization = false;
    private bool tempButtonSouthDelay = false; // E3 Hack: Fix starting game when exiting customization

    private void Start()
    {
        AetherInput.GetUIActions().Submit.performed += SubmitInputCallback;
        AetherInput.GetUIActions().Navigate.performed += SwitchIconsCallback;
        AetherInput.GetUIActions().Cancel.performed += SwitchIconsCallback;
        AetherInput.GetUIActions().MiddleClick.performed += SwitchIconsCallback;
        AetherInput.GetUIActions().Click.performed += SwitchIconsCallback;
    }

    private void SwitchIconsCallback(InputAction.CallbackContext ctx)
    {
        string inputButtonType = ctx.control.device.name;

        switch(inputButtonType)
        {
            case "Button": 
                m_FooterAnimator.SetTrigger("ToPS4");
                break;
            
            default: 
                m_FooterAnimator.SetTrigger("ToKeyboard");
                break;
        }
    }


    public void ToggleCustomization()
    {
        tempButtonSouthDelay = !m_IsInCustomization;
        m_UIAnimator.SetBool("ShowCustomization", tempButtonSouthDelay);
    }

    private void SubmitInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_IsInCustomization)
        {
            StartGame();
        }
    }

    private void Update()
    {
        Gamepad gamePad = Gamepad.current;

        if (gamePad != null && !m_IsInCustomization)
        {
            if (gamePad.buttonNorth.wasPressedThisFrame && !m_IsInCustomization)
            {
                ToggleCustomization();
            }
        }
    }

    public void LateUpdate()
    {
        // E3 Hack
        m_IsInCustomization = tempButtonSouthDelay;
    }

    public void StartGame()
    {
        AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
        m_ScreenFadeAnimator.SetTrigger("ToBlack");
        StartCoroutine("StartGameAfterFade");
    }

    private IEnumerator StartGameAfterFade()
    {
        yield return new WaitForSeconds(1.5f);
        m_LobbySystem.OnStart();
    }
}
