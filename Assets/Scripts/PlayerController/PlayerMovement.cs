using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
        public float m_GroundCheckRadius = 0.5f;
        public LayerMask m_GroundLayerMask = new LayerMask();
        public bool m_IsGrounded = false;
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
    public class DodgeParams
    {
        public bool m_IsDodging;
        public bool m_DodgedInCurrentFrame;
        public bool m_IsBackwardsDodge;
        public float m_DodgeTimeTotal = 0.65f;
        public float m_BackDodgeTimeTotal = 0.2f;
        public float m_DodgeDelay = 0.15f;
        public float m_DodgeSpeed = 10;
        public float m_DodgeCooldown = 0.5f;
        public float m_LastCompletedDodgeTime;
        public Vector3 m_DodgeDirection;
    }

    #region References
    private CharacterController m_CharacterController;
    private PlayerStance m_PlayerStance;
    private PlayerCombatHandler m_PlayerCombatHandler;
    #endregion

    #region Params
    [SerializeField] private MovementParams m_MovementParams;
    [SerializeField] private GroundingParams m_GroundingParams;
    [SerializeField] private GravityParams m_GravityParams;
    [SerializeField] private JumpParams m_JumpParams;
    [SerializeField] private DodgeParams m_DodgeParams;
    #endregion

    // TODO: Move this into Player
    private bool m_IsDead;

    private bool m_IsDamaged;

    private bool m_IsFrozen;

    void Start()
    {
        AetherInput.GetPlayerActions().Jump.performed += JumpInputCallback;
        AetherInput.GetPlayerActions().Dodge.performed += DodgeInputCallback;
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerStance = GetComponent<PlayerStance>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
            
        ComputeYAxisVelocity();
        ComputeXZAxisVelocity();
        m_CharacterController.Move(ComputeMovementDelta());
    }

    public void ToggleDead() 
    {
        m_IsDead = !m_IsDead;
    }

    public void ToggleDamaged() 
    {
        m_IsDamaged = !m_IsDamaged;
    }

    public void SetFrozen(bool frozen)
    {
        m_IsFrozen = frozen;
    }

    public bool IsDead()
    {
        return m_IsDead;
    }

    public bool IsDamaged()
    {
        return m_IsDamaged;
    }

    public bool IsFrozen()
    {
        return m_IsFrozen;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_GroundingParams.m_GroundCheckPosition.position, m_GroundingParams.m_GroundCheckRadius);
    }

    private void LateUpdate()
    {
        m_DodgeParams.m_DodgedInCurrentFrame = false;
    }

    private void ComputeYAxisVelocity()
    {
        // Update grounded flag
        m_GroundingParams.m_IsGrounded = Physics.CheckSphere(m_GroundingParams.m_GroundCheckPosition.position,
            m_GroundingParams.m_GroundCheckRadius, m_GroundingParams.m_GroundLayerMask);

        if (IsGrounded() && m_MovementParams.m_Velocity.y <= 0.0f)
        {
            m_MovementParams.m_Velocity.y = -2.0f;
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
            m_MovementParams.m_Velocity.x = Mathf.Lerp(m_MovementParams.m_Velocity.x, 0, Time.deltaTime * 5);
            m_MovementParams.m_Velocity.z = Mathf.Lerp(m_MovementParams.m_Velocity.z, 0, Time.deltaTime * 5);
            return;
        }

        Vector2 inputXZ = GetInputAxis();
        Vector3 xzVelocity = Camera.main.transform.right * inputXZ[0] + Camera.main.transform.forward * inputXZ[1];
        xzVelocity *= m_MovementParams.m_MoveSpeed;
        xzVelocity.y = 0;

        // Slow the player down when walking backwards or side to side
        if (m_PlayerStance.IsCombatStance() && !IsMovingForward())
            xzVelocity *= m_MovementParams.m_NonForwardMovePenalty.Evaluate(Mathf.Abs(VelocityDotForward()));

        // Slow the player down when blocking
        if (m_PlayerCombatHandler.IsBlocking())
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

    private void RotatePlayer()
    {
        // Overrides
        if (IsDodging())
        {
            if (!m_DodgeParams.m_IsBackwardsDodge)
                RotateTowardsDirection(m_DodgeParams.m_DodgeDirection);
            else
            {
                // Don't rotate
            }
        }
        // Last two entries are defaults
        else if (m_PlayerStance.IsCombatStance())
        {
            Vector3 cameraLookAt = Camera.main.transform.forward;
            cameraLookAt.y = 0;
            RotateTowardsDirection(cameraLookAt);
        }
        else
        {
            RotateTowardsMovement();
        }
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized), Time.deltaTime * 10);
    }

    private void RotateTowardsMovement()
    {
        Vector3 velocity = GetXZVelocity();

        if (velocity.magnitude > 0.0f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity.normalized), Time.deltaTime * 10);
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

    public void DodgeInputCallback(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!CanDodge())
            return;

        // Is a backwards dash (driven by root motion)
        m_DodgeParams.m_IsDodging = true;
        m_DodgeParams.m_DodgedInCurrentFrame = true;
        System.Action onCompleteDodge = () =>
        {
            m_DodgeParams.m_LastCompletedDodgeTime = Time.time;
            m_DodgeParams.m_IsDodging = false;
            m_DodgeParams.m_IsBackwardsDodge = false;
        };

        if (GetInputAxis()[1] <= 0)
            DoBackwardDodge(onCompleteDodge);
        else
            DoForwardDodge(onCompleteDodge);
    }

    private bool CanDodge()
    {
        if (!m_PlayerStance.IsCombatStance())
            return false;

        if (Time.time - m_DodgeParams.m_LastCompletedDodgeTime < m_DodgeParams.m_DodgeCooldown)
            return false;

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_DODGE) && IsMovingForward())
            return false;

        if (!m_PlayerStance.CanPerformAction(PlayerStance.Action.ACTION_DODGEBACK) && !IsMovingForward())
            return false;

        // Forward speed condition for dodging forward
        if (IsMovingForward() && GetInputAxis().magnitude < 0.5f)
            return false;

        // Backward direction condition for dodging backward
        if (!IsMovingForward() && Mathf.Abs(GetInputAxis().x) > 0.5f)
            return false;

        return true;
    }

    private void DoForwardDodge(System.Action onCompleteDodge)
    {
        // Set dodge params
        m_DodgeParams.m_IsBackwardsDodge = false;

        // Not a backwards dash, then its a forward dodge/roll
        Vector3 dashDirection = GetXZVelocity().normalized;

        // TODO: "Velocity" is technically 0 immediately after a backwards dash, so it is hard to 
        // a backward + forward dash right after for style points
        // Possible fixes are: delay repeated dashes (done), read only controller input for dash (but must
        // convert to actual direction)
        if (dashDirection.magnitude == 0)
        {
            dashDirection = transform.forward;
        }

        Debug.Assert(dashDirection != Vector3.zero);
        // This value is used by animation systems to rotate the player in the right direction
        m_DodgeParams.m_DodgeDirection = dashDirection;

        StartCoroutine(Dash(dashDirection, m_DodgeParams.m_DodgeDelay, m_DodgeParams.m_DodgeTimeTotal,
            m_DodgeParams.m_DodgeSpeed, onCompleteDodge));
    }

    private void DoBackwardDodge(System.Action onCompleteDodge)
    {
        // Set dodge params
        m_DodgeParams.m_IsBackwardsDodge = true;
        StartCoroutine(Wait(m_DodgeParams.m_BackDodgeTimeTotal, onCompleteDodge));
    }

    public IEnumerator Dash(Vector3 direction, float delay, float duration, float speed, System.Action onComplete)
    {
        // Let the wind up animation play (this should really be an animation callback @TODO)
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            // Send the player flying
            m_CharacterController.Move(direction * speed * Time.deltaTime);
            yield return null;
        }

        onComplete?.Invoke();
    }

    IEnumerator Wait(float duration, System.Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    public Vector2 GetInputAxis()
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

    public void RootMotionMoveTo(Vector3 rootPosition, Quaternion rootRotation)
    {
        m_MovementParams.m_Velocity.x = 0;
        m_MovementParams.m_Velocity.z = 0;
        Vector3 moveAmount = rootPosition - transform.position;
        m_CharacterController.Move(moveAmount * 2); // Hack
        transform.rotation = rootRotation;
    }

    public bool IsGrounded()
    {
        return m_GroundingParams.m_IsGrounded;
    }

    public bool JumpedInCurrentFrame()
    {
        return m_JumpParams.m_JumpedInCurrentFrame;
    }

    public bool IsDodging()
    {
        return m_DodgeParams.m_IsDodging;
    }

    public bool IsDodgingBackwards()
    {
        return m_DodgeParams.m_IsBackwardsDodge;
    }

    public bool DodgedBackwardsInCurrentFrame()
    {
        return m_DodgeParams.m_IsBackwardsDodge && m_DodgeParams.m_DodgedInCurrentFrame;
    }

    public bool DodgedInCurrentFrame()
    {
        return m_DodgeParams.m_DodgedInCurrentFrame;
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
