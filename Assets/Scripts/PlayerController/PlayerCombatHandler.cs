using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.HID.HID;

[RequireComponent(typeof(PlayerStance))]
public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerStance m_PlayerStance;

    // For animation lookup
    private bool m_AttackedInCurrentFrame;
    private bool m_BlockedInCurrentFrame;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerStance = GetComponent<PlayerStance>();
        AetherInput.GetPlayerActions().Fire.performed += Attack;
        AetherInput.GetPlayerActions().Sheathe.performed += SheatheWeapon;
        AetherInput.GetPlayerActions().Block.performed += Block;
    }

    private void LateUpdate()
    {
        m_AttackedInCurrentFrame = false;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_PlayerStance.IsCombatStance())
        {
            m_PlayerStance.SetStance(PlayerStance.Stance.STANCE_TWOHANDED);
            return;
        }

        m_AttackedInCurrentFrame = true;
    }
    private void Block(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;

        if (!button.wasPressedThisFrame)
        {
            m_BlockedInCurrentFrame = false;
            return;
        }

        if (!m_PlayerStance.IsCombatStance())
        {
            m_BlockedInCurrentFrame = false;
            return;
        }

        m_BlockedInCurrentFrame = button.isPressed;
    }


    public bool GetAttackedInCurrentFrame()
    {
        return m_AttackedInCurrentFrame;
    }

    public bool GetBlockedInCurrentFrame()
    {
        if (GetAttackedInCurrentFrame())
            return false;

        return m_BlockedInCurrentFrame;
    }

    public void SheatheWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            m_PlayerStance.SetStance(PlayerStance.Stance.STANCE_UNARMED);
    }
}
