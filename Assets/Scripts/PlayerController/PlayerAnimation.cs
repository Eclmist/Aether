using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private AnimationCurve m_AnimationSpeedCurve;

    private PlayerMovement m_PlayerMovement;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float worldVelocity = m_PlayerMovement.GetAbsInput();
        m_Animator.SetFloat("Velocity", worldVelocity);

        if (worldVelocity > 0.01f)
            m_Animator.speed = m_AnimationSpeedCurve.Evaluate(worldVelocity); // Make running animation speed match actual movement speed
        else 
            m_Animator.speed = 1;

        Vector3 movementDir = m_PlayerMovement.GetMovementDirection();
        if (movementDir.magnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDir), Time.deltaTime * 10);
    }
}
