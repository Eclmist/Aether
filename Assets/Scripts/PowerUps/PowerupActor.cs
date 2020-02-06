using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActor : MonoBehaviour
{
    private bool m_canDoubleSpeed;
    private bool m_canDoubleJump;

    private const float m_doubleBuffDuration = 5.0f;

    public bool GetDoubleSpeed()
    {
        return m_canDoubleSpeed;
    }

    public void SetDoubleSpeed(bool boolean)
    {
        m_canDoubleSpeed = boolean;
    }

    public bool GetDoubleJump()
    {
        return m_canDoubleJump;
    }

    public void SetDoubleJump(bool boolean)
    {
        m_canDoubleJump = boolean;
    }

    public void GoFaster()
    {
        StartCoroutine("DoubleUpSpeed");
    }

    public void JumpHigher()
    {
        StartCoroutine("DoubleUpJump");
    }

    IEnumerator DoubleUpJump()
    {
        SetDoubleJump(true);
        yield return new WaitForSeconds(m_doubleBuffDuration);
        SetDoubleJump(false);
    }

    IEnumerator DoubleUpSpeed()
    {
        SetDoubleSpeed(true);
        yield return new WaitForSeconds(m_doubleBuffDuration);
        SetDoubleSpeed(false);
    }
}