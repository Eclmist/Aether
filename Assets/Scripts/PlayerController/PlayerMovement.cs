using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    public class MovementParams
    {
        [Range(0, 10)] public float m_MoveSpeed = 5;
        [Range(0, 1)] public float m_BlockMovePenalty = 0.35f;
        public AnimationCurve m_NonForwardMovePenalty;

        public Vector3 m_Velocity;
        public Vector3 m_ExternalVelocityModifier = Vector3.one;
    }

    [System.Serializable]
    public class GroundingParams
    {
        public Transform m_GroundCheckPosition = null;
        public LayerMask m_GroundLayerMask = new LayerMask();
        public bool m_IsGrounded = false;

        public void Update()
        {
            m_IsGrounded = Physics.CheckSphere(m_GroundCheckPosition.position, 0.5f, m_GroundLayerMask);
        }
    }

    [System.Serializable]
    public class GravityParams
    {
        [Range(0, -50)] public float m_Gravity = -30.0f;
        public float m_FallingGravityMultiplier = 1.5f;
        public float m_ExternalGravityModifier = 1.0f;
    }

    [System.Serializable]
    public class JumpParams
    {
        public float m_JumpHeight = 3.5f;
        public float m_ExternalJumpHeightModifier = 1.0f;
        public bool m_JumpedInCurrentFrame;
    }

    [System.Serializable]
    public class DashParams
    {
        public bool m_IsDashing;
        public bool m_DashedInCurrentFrame;
        public float m_DashTimeTotal = 0.5f;
        public float m_DashDelay = 0.3f;
        public float m_DashSpeed = 9;
        public int m_DashDirectionAnimationIndex;
    }

    #region References
    private CharacterController m_CharacterController;
    private Player m_Player;
    private PlayerStance m_PlayerStance;
    private PlayerCombatHandler m_PlayerCombatHandler;
    #endregion

    #region Params
    [SerializeField] private MovementParams m_MovementParams;
    [SerializeField] private GroundingParams m_GroundingParams;
    [SerializeField] private GravityParams m_GravityParams;
    [SerializeField] private JumpParams m_JumpParams;
    [SerializeField] private DashParams m_DashParams;
    #endregion

    void Start()
    {
        AetherInput.GetPlayerActions().Jump.performed += JumpInputCallback;
        AetherInput.GetPlayerActions().Roll.performed += DashInputCallback;
        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GetComponent<Player>();
        m_PlayerStance = GetComponent<PlayerStance>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        ComputeYAxisVelocity();
        ComputeXZAxisVelocity();

        m_CharacterController.Move(ComputeMovementDelta());

        if (m_Player.networkObject != null)
            m_Player.networkObject.position = transform.position;
    }

    private void ComputeYAxisVelocity()
    {
        // Update grounded flag
        m_GroundingParams.m_IsGrounded = Physics.CheckSphere(m_GroundingParams.m_GroundCheckPosition.position,
            0.001f, m_GroundingParams.m_GroundLayerMask);

        if (IsGrounded() && m_MovementParams.m_Velocity.y <= 0.0f)
        {
            m_MovementParams.m_Velocity.y = 0.0f;
        }
        else
        {
            // Accumulate down velocity
            m_MovementParams.m_Velocity.y += GetGravityMagnitude() * Time.deltaTime;
        }
    }
    
    private void ComputeXZAxisVelocity()
    {
        // If the player is not allowed to walk, don't.
        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_WALK))
        {
            m_MovementParams.m_Velocity.x = 0;
            m_MovementParams.m_Velocity.z = 0;
            return;
        }

        Vector2 inputXZ = GetInputAxisXZ();
        Vector3 xzVelocity = Camera.main.transform.right * inputXZ[0] + Camera.main.transform.forward * inputXZ[1];
        xzVelocity *= m_MovementParams.m_MoveSpeed;
        xzVelocity.y = 0;

        // Slow the player down when walking backwards or side to side
        if (m_PlayerStance.IsCombatStance() && !IsMovingForward())
            xzVelocity *= m_MovementParams.m_NonForwardMovePenalty.Evaluate(Mathf.Abs(VelocityDotForward()));

        // Slow the player down when blocking
        if (m_PlayerCombatHandler.GetBlockedInCurrentFrame())
            xzVelocity *= (1 - m_MovementParams.m_BlockMovePenalty);

        // Add external modifier;
        xzVelocity.Scale(m_MovementParams.m_ExternalVelocityModifier);

        // Update movement params so other system can use this final value.
        m_MovementParams.m_Velocity.x = xzVelocity.x;
        m_MovementParams.m_Velocity.z = xzVelocity.z;
    }

    private Vector3 ComputeMovementDelta()
    {
        Vector3 deltaPos = GetXZVelocity() * Time.deltaTime;
        float Vy = m_MovementParams.m_Velocity.y;

        // Kinematic equation for constant acceleration
        float t = Time.deltaTime;
        float t2 = t * t;

        deltaPos.y = Vy * t + 0.5f * GetGravityMagnitude() * t2;

        return deltaPos;
    }

    public void JumpInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_JUMP))
            return;

        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        // Make the player jump. Velocity is computed with kinematic equation.
        m_MovementParams.m_Velocity.y = 
            Mathf.Sqrt(m_JumpParams.m_JumpHeight * m_JumpParams.m_ExternalJumpHeightModifier * -2 * GetGravityMagnitude());

        m_JumpParams.m_JumpedInCurrentFrame = true;
        yield return new WaitForEndOfFrame();
        m_JumpParams.m_JumpedInCurrentFrame = false;
    }

    public void DashInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_PlayerStance.IsCombatStance())
            return;

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_DASH))
            return;

        if (VelocityDotForward() < 0.0f)
            return;

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        m_DashParams.m_IsDashing = true;

        // Compute the absolute dash direction for animator use (left, right, forward)
        if (VelocityDotForward() > 0.9)
            m_DashParams.m_DashDirectionAnimationIndex = 1;
        else if (Vector3.Cross(GetXZVelocity(), transform.forward).y < 0)
            m_DashParams.m_DashDirectionAnimationIndex = 2;
        else
            m_DashParams.m_DashDirectionAnimationIndex = 0;

        Vector3 dashDirection = GetXZVelocity().normalized;

        float startTime = Time.time;

        m_DashParams.m_DashedInCurrentFrame = true;
        yield return new WaitForEndOfFrame();
        m_DashParams.m_DashedInCurrentFrame = false;

        while (Time.time - startTime < m_DashParams.m_DashTimeTotal)
        {
            // Dash animation has not yet started, don't make the player dash yet.
            if (Time.time - startTime < m_DashParams.m_DashDelay)
            {
                yield return null;
                continue;
            }
            else
            {
                // Send the player flying
                m_CharacterController.Move(dashDirection * m_DashParams.m_DashSpeed * Time.deltaTime);
                yield return null;
            }
        }

        m_DashParams.m_IsDashing = false;
    }

    public Vector2 GetInputAxisXZ()
    {
        return AetherInput.GetPlayerActions().Move.ReadValue<Vector2>();
    }

    public Vector3 GetVelocity()
    {
        return m_MovementParams.m_Velocity;
    }

    public float GetGravityMagnitude()
    {
        float finalGravity = m_GravityParams.m_Gravity;

        if (m_MovementParams.m_Velocity.y < 0)
            finalGravity *= m_GravityParams.m_FallingGravityMultiplier;

        finalGravity *= m_GravityParams.m_ExternalGravityModifier;
        return finalGravity;
    }

    public bool IsGrounded()
    {
        return m_GroundingParams.m_IsGrounded;
    }

    public bool JumpedInCurrentFrame()
    {
        return m_JumpParams.m_JumpedInCurrentFrame;
    }

    public bool IsDashing()
    {
        return m_DashParams.m_IsDashing;
    }

    public bool DashedInCurrentFrame()
    {
        return m_DashParams.m_DashedInCurrentFrame;
    }

    public int GetDashDirectionIndex()
    {
        return m_DashParams.m_DashDirectionAnimationIndex;
    }

    public bool IsMovingForward()
    {
        return VelocityDotForward() > 0;
    }

    public void SetExternalVelocityModifier(Vector3 velocityModifier)
    {
        m_MovementParams.m_ExternalVelocityModifier = velocityModifier;
    }

    public void SetExternalGravityModifier(float modifier)
    {
        m_GravityParams.m_ExternalGravityModifier = modifier;
    }

    public void SetExternalJumpHeightModifier(float modifier)
    {
        m_JumpParams.m_ExternalJumpHeightModifier = modifier;
    }

    public Vector3 GetXZVelocity()
    {
        return Vector3.Scale(m_MovementParams.m_Velocity, Vector3.one - Vector3.up);
    }

    private float VelocityDotForward()
    {
        return Vector3.Dot(transform.forward, GetXZVelocity().normalized);
    }
}
