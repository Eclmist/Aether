using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(Player))]
public class PlayerNetworkHandler : MonoBehaviour
{
    public event System.Action<Player> PlayerDied;

    private PlayerNetworkObject m_PlayerNetworkObject;

    [SerializeField]
    private Animator m_Animator;
    private HealthHandler m_HealthHandler;
    private SkillHandler m_SkillHandler;
    private Player m_Player;

    // Local player only scripts
    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;
    private PlayerStance m_PlayerStance;
    private PlayerCombatHandler m_PlayerCombatHandler;

    private void Start()
    {
        m_Player = GetComponent<Player>();
        m_PlayerNetworkObject = m_Player.networkObject;
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_HealthHandler = GetComponent<HealthHandler>();
        m_SkillHandler = GetComponent<SkillHandler>();
        m_PlayerStance = GetComponent<PlayerStance>();
        m_PlayerCombatHandler = GetComponent<PlayerCombatHandler>();

        // Make sure animator exists
        Debug.Assert(m_Animator != null, "Animator should not be null");
    }

    private void Update()
    {
        if (m_PlayerNetworkObject == null)
            return;

        if (m_PlayerNetworkObject.IsOwner)
        {
            if (m_PlayerMovement == null || m_PlayerAnimation == null || m_HealthHandler == null || m_PlayerStance == null)
            {
                Debug.LogWarning("Movement/Animation/Health/Stance script not found on local player");
                return;
            }

            // Send movement state
            m_PlayerNetworkObject.position = transform.position;
            m_PlayerNetworkObject.rotation = transform.rotation;

            // Send animation state
            m_PlayerNetworkObject.axisDelta = m_PlayerMovement.GetInputAxis();
            m_PlayerNetworkObject.vertVelocity = m_PlayerMovement.GetVelocity().y;
            m_PlayerNetworkObject.grounded = m_PlayerMovement.IsGrounded();

            // Combat states
            m_PlayerNetworkObject.weaponIndex = (int)m_PlayerStance.GetStance();
            m_PlayerNetworkObject.blocked = m_PlayerCombatHandler.IsBlocking();
            m_PlayerNetworkObject.skillIndex = m_SkillHandler.GetCurrentActiveSkill(); 

            if (m_PlayerCombatHandler.GetAttackedInCurrentFrame())
            {
                Debug.Assert(m_PlayerNetworkObject.weaponIndex != 0, "Attacking unarmed, wtf?");
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_ATTACK, Receivers.All);
            }

            if (m_PlayerMovement.DodgedBackwardsInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_BACK_DASH, Receivers.All);
            else if (m_PlayerMovement.DodgedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_DASH, Receivers.All);


            if (m_PlayerMovement.JumpedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_JUMP, Receivers.All);

            // Send health state
            if (m_HealthHandler.DamagedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_DAMAGED, Receivers.All);
        }
        else
        {
            // Receive movement state
            transform.position = m_PlayerNetworkObject.position;
            transform.rotation = m_PlayerNetworkObject.rotation;

            // Receive animation state
            if (m_Animator == null)
            {
                Debug.LogWarning("Animator does not exist on player");
                return;
            }
            m_Animator.SetFloat("Velocity-XZ-Normalized-01", m_PlayerNetworkObject.axisDelta.magnitude);
            m_Animator.SetFloat("Velocity-X-Normalized", m_PlayerNetworkObject.axisDelta.x);
            m_Animator.SetFloat("Velocity-Z-Normalized", m_PlayerNetworkObject.axisDelta.y);
            m_Animator.SetFloat("Velocity-Y-Normalized", m_PlayerNetworkObject.vertVelocity);
            m_Animator.SetBool("Grounded", m_PlayerNetworkObject.grounded);
            m_Animator.SetInteger("WeaponIndex", m_PlayerNetworkObject.weaponIndex);
            m_Animator.SetBool("Block", m_PlayerNetworkObject.blocked);
            m_Animator.SetInteger("SkillsIndex", m_PlayerNetworkObject.skillIndex);

            // Show and hide sword
            // TODO: add delay or make it work with animation callbacks
            m_Player.SetWeaponActive(m_PlayerNetworkObject.weaponIndex != 0);
        }
    }

    public void TriggerDamaged()
    {
        GetKnockBack();
    }

    public void TriggerJump()
    {
        if (m_Animator == null)
        {
            Debug.LogWarning("Animator does not exist on player");
            return;
        }
        m_Animator.SetTrigger("Jump");
    }
    
    public void TriggerAttack()
    {
        if (m_Animator == null)
        {
            Debug.LogWarning("Animator does not exist on player");
            return;
        }
        m_Animator.SetTrigger("Attack");
    }

    public void TriggerDash()
    {
        if (m_Animator == null)
        {
            Debug.LogWarning("Animator does not exist on player");
            return;
        }

        m_Animator.SetTrigger("Roll");
    }

    public void TriggerBackDash()
    {
        if (m_Animator == null)
        {
            Debug.LogWarning("Animator does not exist on player");
            return;
        }

        m_Animator.SetTrigger("Backstep");
    }

    public void SetLocalPlayerPosition(Vector3 position)
    {
        if (m_PlayerNetworkObject != null && !m_PlayerNetworkObject.IsOwner)
            return;

        Shader.SetGlobalVector("_LocalPlayerPosition", transform.position);
    }

    public void TriggerRespawn(System.Action callback)
    {
        StartCoroutine(RespawnSequence(callback));
    }

    private IEnumerator RespawnSequence(System.Action callback)
    {
        if (m_PlayerNetworkObject == null)
            yield break;

        // Set up new position
        Transform spawnPos = GameManager.Instance.GetRespawnPoint();
        CharacterController cc = GetComponent<CharacterController>();

        cc.enabled = false;
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;

        m_PlayerNetworkObject.positionInterpolation.Enabled = false;
        m_PlayerNetworkObject.positionChanged += m_Player.WarpToFirstPosition;

        // Re-enable controller
        yield return new WaitForSeconds(0.2f);
        callback?.Invoke();
        cc.enabled = true;
    }

    private void GetKnockBack()
    {
        m_PlayerMovement.Dash(-1 * transform.forward, 0, 0.5f, 20, () => { });
    }

}
