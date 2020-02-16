using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    [Range(0, 10)]
    private float m_MoveSpeed = 6;

    [SerializeField]
    private AnimationCurve m_NonForwardMovePenalty;

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

    private Player m_Player;
    private PlayerStance m_PlayerStance;

    private Vector3 m_Velocity;
    private Vector3 m_ExternalVelocityModifier = Vector3.one;
    private Vector2 m_LastKnownInput;

    // Dashing
    private bool m_IsDashing;
    private bool m_DashedInCurrentFrame;
    private float m_DashTimeTotal = 0.7f;
    private float m_DashTimeCurrent = 0;
    private float m_DashDelay = 0.2f;
    private float m_DashSpeed = 9;
    private Vector3 m_DashDirection;
    private int m_DashDirectionAnimationIndex;

    void Start()
    {
        AetherInput.GetPlayerActions().Jump.performed += HandleJump;
        AetherInput.GetPlayerActions().Roll.performed += HandleDash;
        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GetComponent<Player>();
        m_PlayerStance = GetComponent<PlayerStance>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            // Gravity should not accumulate when player is grounded. We set velocity to -2 instead of 0
            // because the collision may register before we actually fully touch the ground. This value is purely
            // empirical.
            if (m_Velocity.y < 0)
            {
                m_Velocity.y = -2.0f;
            }
        }

        HandleGravity();

        // Movement overrides
        if (HandleMovementOverrides())
            return;

        HandleMovement();

        float t = Time.deltaTime;
        float t2 = t * t;

        Vector3 finalVelocity = GetVelocity();

        // Kinematic equation for constant acceleration
        float yVelocity = m_Velocity.y * t + 0.5f * GetGravityMagnitude() * t2;
        finalVelocity.y = yVelocity; // E3: For some reason setting the .y doesn't work

        m_CharacterController.Move(finalVelocity);


        if (m_Player.networkObject != null)
            m_Player.networkObject.position = transform.position;
    }

    private void LateUpdate()
    {
        m_DashedInCurrentFrame = false;
    }

    private void HandleGravity()
    {
        m_Velocity.y += GetGravityMagnitude() * Time.deltaTime;
    }

    private bool HandleMovementOverrides()
    {
        if (m_IsDashing)
        {
            if (m_DashTimeCurrent >= m_DashTimeTotal)
            {
                m_IsDashing = false;
                m_DashTimeCurrent = 0;
                return false;
            }

            m_DashTimeCurrent += Time.deltaTime;

            if (m_DashTimeCurrent <= m_DashDelay)
                return true;

            m_CharacterController.Move(m_DashDirection * m_DashSpeed * Time.deltaTime);

            return true;
        }

        return false;
    }
    
    private void HandleMovement()
    {
        m_LastKnownInput = AetherInput.GetPlayerActions().Move.ReadValue<Vector2>();
        Vector3 move = Camera.main.transform.right * m_LastKnownInput.x + Camera.main.transform.forward * m_LastKnownInput.y;
        move *= Time.deltaTime * m_MoveSpeed;
        m_Velocity = new Vector3(move.x, m_Velocity.y, move.z);
    }

    public void HandleJump(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!IsGrounded())
            return;

        m_Velocity.y = Mathf.Sqrt(m_JumpHeight * m_ExternalJumpHeightModifier * -2 * m_Gravity);
    }

    public void HandleDash(InputAction.CallbackContext ctx)
    {
        if (!m_PlayerStance.IsCombatStance())
            return;

        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (m_IsDashing)
            return;

        float vdf = VelocityDotForward();

        if (vdf < 0.0f)
            return;

        m_IsDashing = true;
        m_DashedInCurrentFrame = true;

        // Compute the absolute dash direction for animation (left, right, forward)
        if (vdf > 0.9)
            m_DashDirectionAnimationIndex = 1;
        else if (Vector3.Cross(GetXZVelocity(), transform.forward).y < 0)
            m_DashDirectionAnimationIndex = 2;
        else
            m_DashDirectionAnimationIndex = 0;

        m_DashDirection = GetXZVelocity().normalized;
    }

    public Vector2 GetLastKnownInput()
    {
        return m_LastKnownInput;
    }

    public Vector3 GetVelocity()
    {
        Vector3 finalVelocity = m_Velocity;

        // Slow the player down when walking backwards or side to side
        if (m_PlayerStance.IsCombatStance() && !IsWalkingForward())
            finalVelocity *= m_NonForwardMovePenalty.Evaluate(Mathf.Abs(VelocityDotForward()));

        // Add external modifier;
        finalVelocity.Scale(m_ExternalVelocityModifier);

        return finalVelocity;
    }

    public float GetGravityMagnitude()
    {
        return m_Gravity * (m_Velocity.y >= 0 ? 1 : m_FallingGravityMultiplier) * m_ExternalGravityModifier;
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(m_GroundCheck.position, 0.5f, m_LayerMask);
    }

    public bool IsDashing()
    {
        return m_IsDashing;
    }

    public bool DashedInCurrentFrame()
    {
        return m_DashedInCurrentFrame;
    }

    public int GetDashDirectionIndex()
    {
        return m_DashDirectionAnimationIndex;
    }

    public bool IsWalkingForward()
    {
        return VelocityDotForward() > 0;
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

    private Vector3 GetXZVelocity()
    {
        return new Vector3(m_Velocity.x, 0, m_Velocity.z);
    }

    private float VelocityDotForward()
    {
        return Vector3.Dot(transform.forward, GetXZVelocity().normalized);
    }
}
