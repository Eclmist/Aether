using System.Collections;
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
    private LobbySystem m_LobbySystem;

    private bool m_IsInCustomization;

    public void Start()
    {
        AetherInput.GetUIActions().Customize.performed += ToggleCustomizationCallback;
        AetherInput.GetUIActions().Submit.performed += SubmitInputCallback;
    }

    public void ToggleCustomizationCallback(InputAction.CallbackContext ctx)
    {
        ToggleCustomization();
    }

    public void ToggleCustomization()
    {
        m_IsInCustomization = !m_IsInCustomization;
        AudioManager.m_Instance.PlaySound("GEN_Success_2", 1.0f, 1.0f);
        m_UIAnimator.SetBool("ShowCustomization", m_IsInCustomization);
    }

    private void SubmitInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        InitiateGame();
    }

    public void InitiateGame()
    {
        if (!m_IsInCustomization)
            StartGame();
    }
    private void StartGame()
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
