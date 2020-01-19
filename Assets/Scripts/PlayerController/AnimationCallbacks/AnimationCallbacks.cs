using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationCallbacks : MonoBehaviour
{
    private Animator m_Animator;

    [SerializeField]
    private PlayerMovement m_PlayerMovement;

    protected void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void JumpStartCompleted()
    {
        m_Animator.SetBool("Jumping", false);
        m_PlayerMovement.Jump();
    }

    public void OnCallChangeFace(string target)
    {

    }
}
