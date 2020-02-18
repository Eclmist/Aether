using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationCallbacks : MonoBehaviour
{
    private Animator m_Animator;

    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    [SerializeField]
    private PlayerStance m_PlayerStance;

    protected void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void JumpStartCompleted()
    {
    }

    public void OnCallChangeFace(string target)
    {

    }

    public void SetWeaponActive()
    {
        if (m_PlayerStance != null)
            m_PlayerStance.SetWeaponActive();
    }

    public void OnAnimatorMove()
    {
        if (!m_Animator)
            return;

        if (!m_Animator.GetCurrentAnimatorStateInfo(4).IsTag("ApplyRootMotion"))
            return;

        m_PlayerMovement.RootMotionMoveTo(m_Animator.rootPosition, m_Animator.rootRotation);
    }
}
