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
}
