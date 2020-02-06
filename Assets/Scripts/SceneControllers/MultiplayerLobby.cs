using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerLobby : MonoBehaviour
{
    [SerializeField]
    public Animator m_UIAnimator;

    private bool m_IsInCustomization = false;

    public void ToggleCustomization()
    {
        m_IsInCustomization = !m_IsInCustomization;
        m_UIAnimator.SetBool("ShowCustomization", m_IsInCustomization);
    }

    public void Update()
    {
        Gamepad gamePad = Gamepad.current;
        if (gamePad != null)
        {
            if (gamePad.buttonNorth.wasPressedThisFrame && !m_IsInCustomization)
            {
                ToggleCustomization();
            }
        }
    }
}
