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
        AetherInput.GetPlayerActions().Fire.performed += AttackInputCallback;
        AetherInput.GetPlayerActions().Sheathe.performed += SheatheInputCallback;
    }

    private void LateUpdate()
    {
        m_AttackedInCurrentFrame = false;
    }

    private void Update()
    {
        // "Held Down" Inputs should be checked in update
        BlockIfPossible();
    }

    private void AttackInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_PlayerStance.IsCombatStance())
        {
            m_PlayerStance.SetStance(PlayerStance.Stance.STANCE_TWOHANDED);
            return;
        }

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_ATTACK))
            return;

        m_AttackedInCurrentFrame = true;
    }
    private void BlockIfPossible()
    {
        m_BlockedInCurrentFrame = false;

        if (!m_PlayerStance.IsCombatStance())
            return;

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_BLOCK))
            return;

        m_BlockedInCurrentFrame = AetherInput.GetPlayerActions().Block.ReadValue<float>() != 0;
    }

    public bool GetAttackedInCurrentFrame()
    {
        return m_AttackedInCurrentFrame;
    }

    public bool IsBlocking()
    {
        if (GetAttackedInCurrentFrame())
            return false;

        return m_BlockedInCurrentFrame;
    }

    public void SheatheInputCallback(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            m_PlayerStance.SetStance(PlayerStance.Stance.STANCE_UNARMED);
    }
}
