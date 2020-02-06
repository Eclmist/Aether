using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerLobby : MonoBehaviour
{
    [SerializeField]
    public Animator m_UIAnimator;

    [SerializeField]
    public LobbySystem m_LobbySystem;

    private bool m_IsInCustomization = false;

    public void ToggleCustomization()
    {
        m_IsInCustomization = !m_IsInCustomization;
        m_UIAnimator.SetBool("ShowCustomization", m_IsInCustomization);
    }

    public void Update()
    {
        Gamepad gamePad = Gamepad.current;

        if (gamePad != null && !m_IsInCustomization)
        {
            if (gamePad.buttonNorth.wasPressedThisFrame)
            {
                ToggleCustomization();
            }

            if (gamePad.buttonSouth.wasPressedThisFrame && !m_IsInCustomization)
            {
                m_LobbySystem.OnStart();
            }
        }
    }
}
