using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Crosshair : MonoBehaviour
{
    private Animator m_Animator;
    public static Crosshair m_Instance;

    private bool m_ShouldUnfocusNextFrame;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Instance != null)
            Destroy(m_Instance);

        m_Instance = this;
        m_Animator = GetComponent<Animator>();

    }

    private void LateUpdate()
    {
        if (m_ShouldUnfocusNextFrame)
        {
            SetFocus(false);
            m_ShouldUnfocusNextFrame = false;
        }
    }

    public void SetFocus(bool focus)
    {
        m_Animator.SetBool("Focused", focus);
    }

    public void SetFocusOneFrame()
    {
        m_Animator.SetTrigger("Focused");
        m_ShouldUnfocusNextFrame = true;
    }
}
