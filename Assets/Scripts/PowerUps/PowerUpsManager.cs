using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PowerUpsManager : MonoBehaviour
{
    private bool m_CanDoubleSpeed;
    private bool m_CanDoubleJump;
    private PlayerMovement m_PlayerMovement;
    private const float m_DoubleBuffDuration = 5.0f;

    private void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }
    private void SetSpeedPowerUp()
    {
        m_CanDoubleSpeed = true;
    }

    private void RemoveSpeedPowerUp()
    {
        m_CanDoubleSpeed = false;
    }

    // Allow other classes to check whether player has power ups
    public bool HasSpeedPowerUp()
    {
        return m_CanDoubleSpeed;
    }

    public void GoFaster()
    {
        StartCoroutine("TriggerPlayerSpeedPowerUp");
    }

    IEnumerator TriggerPlayerSpeedPowerUp()
    {
        //m_PlayerMovement.SetSpeedPowerUpStateTrue();
        SetSpeedPowerUp();
        yield return new WaitForSeconds(m_DoubleBuffDuration);
        //m_PlayerMovement.SetSpeedPowerUpStateFalse();
        RemoveSpeedPowerUp();
    }

    private void SetJumpPowerUp()
    {
        m_CanDoubleJump = true;
    }
    private void RemoveJumpPowerUp()
    {
        m_CanDoubleJump = false;
    }

    public bool HasJumpPowerUp()
    {
        return m_CanDoubleJump;
    }

    public void JumpHigher()
    {
        StartCoroutine("TriggerPlayerJumpPowerUp");
    }

    IEnumerator TriggerPlayerJumpPowerUp()
    {
        //m_PlayerMovement.SetJumpPowerUpStateTrue();
        SetJumpPowerUp();
        yield return new WaitForSeconds(m_DoubleBuffDuration);
        //m_PlayerMovement.SetJumpPowerUpStateFalse();
        RemoveJumpPowerUp();
    }
}