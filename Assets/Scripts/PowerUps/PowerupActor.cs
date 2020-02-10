using System.Collections;
using UnityEngine;

public class PowerupActor : MonoBehaviour
{
    private bool m_CanDoubleSpeed;
    private bool m_CanDoubleJump;

    public bool IsDoubleSpeed() {
        return m_CanDoubleSpeed;
    }

    public bool IsDoubleJump() {
        return m_CanDoubleJump;
    }

    public void GoFaster(float buffDuration)
    {
        if (!m_CanDoubleSpeed) 
        {
            StartCoroutine(DoubleUpSpeed(buffDuration));
        }
    }

    public void JumpHigher(float buffDuration)
    {
        if (!m_CanDoubleJump)
        {
            StartCoroutine(DoubleUpJump(buffDuration));
        }
    }

    IEnumerator DoubleUpJump(float buffDuration)
    {
        m_CanDoubleJump = true;
        yield return new WaitForSeconds(buffDuration);
        m_CanDoubleJump = false;
    }

    IEnumerator DoubleUpSpeed(float buffDuration)
    {
        m_CanDoubleSpeed = true;
        yield return new WaitForSeconds(buffDuration);
        m_CanDoubleSpeed = false;
    }
}