using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerLobby : MonoBehaviour
{
    public Animator m_UIAnimator;
    public Animator m_ScreenFadeAnimator;
    public LobbySystem m_LobbySystem;

    private bool m_IsInCustomization = false;
    private bool tempButtonSouthDelay = false; // E3 Hack: Fix starting game when exiting customization

    public void ToggleCustomization()
    {
        tempButtonSouthDelay = !m_IsInCustomization;
        m_UIAnimator.SetBool("ShowCustomization", tempButtonSouthDelay);
    }

    public void Update()
    {
        Gamepad gamePad = Gamepad.current;

        if (gamePad != null && !m_IsInCustomization)
        {
            if (gamePad.buttonSouth.wasPressedThisFrame && !m_IsInCustomization)
            {
                StartGame();
            }

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
