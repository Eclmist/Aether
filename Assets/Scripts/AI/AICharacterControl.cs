using System;
using UnityEngine;

/*
[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof (CharacterController))]
public class AICharacterControl : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public CharacterController character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for
    
    [Header("Movement")]
    [SerializeField]
    [Range(0, 10)]
    private float m_MoveSpeed = 6;

    [SerializeField]
    private Transform m_GroundCheck = null;

    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private float m_Gravity = -9.8f;
    private float m_ExternalGravityModifier = 1.0f;

    [SerializeField]
    private float m_FallingGravityMultiplier = 2.0f;

    [SerializeField]
    private float m_JumpHeight = 1.3f;
    private float m_ExternalJumpHeightModifier = 1.0f;

    private CharacterController m_CharacterController;

    private Vector3 m_Velocity;
    private Vector3 m_ExternalVelocityModifier = Vector3.one;
    private Vector2 m_LastKnownInput;

    private float m_LandingRecoveryTime = 0.05f;
    private float m_LandingSpeedModifier = 0.0f;
    private float m_LandingTime = 0;
    private bool m_IsMidAir;
    private bool m_JumpedInCurrentFrame;
    private bool m_cannotMove;
    private bool m_HasSpeedPowerUp;
    private bool m_HasJumpPowerUp;
    private bool m_IsParalyzed;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        //m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
        m_IsParalyzed = false;
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<CharacterController>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsParalyzed)
        {
            if (!GetIsGrounded())
            {
                m_CharacterController.Move(new Vector3(-1.0f, -1.0f, -1.0f));
            }
            return;
        }
        if (GetIsGrounded())
        {
            // Gravity should not accumulate when player is grounded. We set velocity to -2 instead of 0
            // because the collision may register before we actually fully touch the ground. This value is purely
            // empirical.
            if (m_Velocity.y < 0)
            {
                m_Velocity.y = -2.0f;
            }

            if (m_IsMidAir)
            {
                m_IsMidAir = false;
                // Landing frame
                m_LandingTime = Time.time;
            }
        }

        HandleGravity();
        HandleMovement();

        float t = Time.deltaTime;
        float t2 = t * t;

        Vector3 finalVelocity = GetVelocity();

        // Kinematic equation for constant acceleration
        float yVelocity = m_Velocity.y * t + 0.5f * GetGravityMagnitude() * t2;
        finalVelocity.y = yVelocity; // E3: For some reason setting the .y doesn't work

        m_CharacterController.Move(finalVelocity);
        
        if (target != null)
            agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity);
        else
            character.Move(Vector3.zero);
    }

    private void LateUpdate()
    {
        m_JumpedInCurrentFrame = false;
    }

    private void HandleGravity()
    {
        m_Velocity.y += GetGravityMagnitude() * Time.deltaTime;
    }
    
    private void HandleMovement()
    {
        m_LastKnownInput = AetherInput.GetPlayerActions().Move.ReadValue<Vector2>();
        Vector3 move = Camera.main.transform.right * m_LastKnownInput.x + Camera.main.transform.forward * m_LastKnownInput.y;
        move *= Time.deltaTime * m_MoveSpeed;

        // Slow the player down after a fall
        if (IsRecoveringFromFall())
        {
            float mod = (Time.time - m_LandingTime) / m_LandingRecoveryTime;
            move *= Mathf.Lerp(m_LandingSpeedModifier, 1, mod);
        }

        m_Velocity = new Vector3(move.x, m_Velocity.y, move.z);
    }

    public void HandleJump(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!GetIsGrounded())
            return;

        if (IsRecoveringFromFall())
            return;

        if (m_cannotMove)
            return;

        m_JumpedInCurrentFrame = true;
        m_IsMidAir = true;
    }

    public Vector2 GetLastKnownInput()
    {
        return m_LastKnownInput;
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(
            m_Velocity.x * m_ExternalVelocityModifier.x,
            m_Velocity.y * m_ExternalVelocityModifier.y,
            m_Velocity.z * m_ExternalVelocityModifier.z);
    }

    public float GetGravityMagnitude()
    {
        return m_Gravity * (m_Velocity.y >= 0 ? 1 : m_FallingGravityMultiplier) * m_ExternalGravityModifier;
    }

    public bool IsRecoveringFromFall()
    {
        return Time.time - m_LandingTime < m_LandingRecoveryTime;
    }

    public bool GetIsGrounded()
    {
        return Physics.CheckSphere(m_GroundCheck.position, 0.5f, m_LayerMask) && !GetJumpedInCurrentFrame();
    }

    public bool GetJumpedInCurrentFrame()
    {
        return m_JumpedInCurrentFrame;
    }

    public void SetUnmovable(bool boolean)
    {
        m_cannotMove = boolean;
    }

    public bool CheckIfUnmovable()
    {
        return m_cannotMove;
    }

    // This should be an animation callback for more visually appealing jumps
    public void Jump()
    {
        m_Velocity.y = Mathf.Sqrt(m_JumpHeight * m_ExternalJumpHeightModifier * -2 * m_Gravity);
    }

    public void SetParalyze()
    {
        m_IsParalyzed = true;
    }

    public void ResetParalyze()
    {
        m_IsParalyzed = false;
    }

    public void SetExternalVelocityModifier(Vector3 velocityModifier)
    {
        m_ExternalVelocityModifier = velocityModifier;
    }

    public void SetExternalGravityModifier(float modifier)
    {
        m_ExternalGravityModifier = modifier;
    }

    public void SetExternalJumpHeightModifier(float modifier)
    {
        m_ExternalJumpHeightModifier = modifier;
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
*/
