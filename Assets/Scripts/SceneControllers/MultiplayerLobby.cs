using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;
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

    private bool m_IsReady = false;

    private bool m_IsInCustomization;

    public void Start()
    {
        AetherInput.GetUIActions().Customize.performed += ToggleCustomizationCallback;
        AetherInput.GetUIActions().Ready.performed += ToggleReadyCallback;
        AetherInput.GetUIActions().Submit.performed += SubmitInputCallback;
    }

    public bool GetIsReady()
    {
        return m_IsReady;
    }

    private void ToggleReadyCallback(InputAction.CallbackContext ctx)
    {
        ToggleReady();
    }

    public void ToggleReady()
    {
        if (m_IsInCustomization)
        {
            AudioManager.m_Instance.PlaySound("Error", 1.0f, 1.0f);
            return;
        }

        m_IsReady = !m_IsReady;
        m_LobbySystem.SetPlayerReady(m_IsReady);
    }

    private void ToggleCustomizationCallback(InputAction.CallbackContext ctx)
    {
        ToggleCustomization();
    }

    // Do not delete, referenced by UI
    public void ToggleCustomization()
    {
        if (m_IsReady)
        {
            AudioManager.m_Instance.PlaySound("Error", 1.0f, 1.0f);
            return;
        }

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
        if (!NetworkManager.Instance.IsServer ||
            m_IsInCustomization ||
            !m_LobbySystem.CanStart())
        {
            AudioManager.m_Instance.PlaySound("Error", 1.0f, 1.0f);
            return;
        }

        // TODO: UIManager/UXManager should handle this
        AudioManager.m_Instance.PlaySound("GEN_Success_1", 1.0f, 1.0f);
        m_ScreenFadeAnimator.SetTrigger("ToBlack");
        StartCoroutine(StartGameAfterFade());
    }

    private IEnumerator StartGameAfterFade()
    {
        yield return new WaitForSeconds(1.5f);
        m_LobbySystem.OnStart();
    }
}
