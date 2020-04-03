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

    // Local player only scripts
    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;

    private void Start()
    {
        m_PlayerNetworkObject = GetComponent<Player>().networkObject;
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_HealthHandler = GetComponent<HealthHandler>();
        m_SkillHandler = GetComponent<SkillHandler>();

        // Make sure animator exists
        Debug.Assert(m_Animator != null, "Animator should not be null");
    }

    private void Update()
    {
        if (m_PlayerNetworkObject == null)
            return;

        if (m_PlayerNetworkObject.IsOwner)
        {
            if (m_PlayerMovement == null || m_PlayerAnimation == null || m_HealthHandler == null)
            {
                Debug.LogWarning("Movement/Animation/Health script not found on local player");
                return;
            }

            // Send movement state
            m_PlayerNetworkObject.position = transform.position;
            m_PlayerNetworkObject.rotation = transform.rotation;

            // Send animation state
            m_PlayerNetworkObject.axisDeltaMagnitude = m_PlayerMovement.GetInputAxis().magnitude;
            m_PlayerNetworkObject.vertVelocity = m_PlayerMovement.GetVelocity().y;
            m_PlayerNetworkObject.grounded = m_PlayerMovement.IsGrounded();

            if (m_PlayerMovement.JumpedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_JUMP, Receivers.All);

            // Send health state
            if (m_HealthHandler.DamagedInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_DAMAGED, Receivers.All);
            if (m_HealthHandler.DeadInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_DEATH, Receivers.All);

            // Send skill state
            if (m_SkillHandler.GetCastInCurrentFrame())
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_SKILL, Receivers.All, m_SkillHandler.GetCurrentActiveSkill());
            else
                m_PlayerNetworkObject.SendRpc(Player.RPC_TRIGGER_SKILL, Receivers.All, (int)SkillItem.SkillType.NONE);
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
            m_Animator.SetFloat("Velocity-XZ-Normalized-01", m_PlayerNetworkObject.axisDeltaMagnitude);
            m_Animator.SetFloat("Velocity-Y-Normalized", m_PlayerNetworkObject.vertVelocity);
            m_Animator.SetBool("Grounded", m_PlayerNetworkObject.grounded);
        }
    }

    public void TriggerDeath()
    {
        StartCoroutine(ReviveSequence());
    }

    public void TriggerDamaged()
    {
        StartCoroutine(DamagedSequence());
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

    public void TriggerSkills(int currentActiveSkill)
    {
        if (m_Animator == null)
        {
            Debug.LogWarning("Animator does not exist on player");
            return;
        }

        m_Animator.SetInteger("SkillsIndex", currentActiveSkill);
    }

    private IEnumerator ReviveSequence()
    {
        m_PlayerMovement.ToggleDead();

        if (m_PlayerNetworkObject == null)
            yield break;

        // Set up new position
        Player player = GetComponent<Player>();
        PlayerNetworkManager pnm = PlayerManager.Instance.GetPlayerNetworkManager();
        PlayerDetails details = player.GetPlayerDetails();
        Transform spawnPos = pnm.GetSpawnPosition(details.GetTeam(), details.GetPosition());
        CharacterController cc = GetComponent<CharacterController>();

        cc.enabled = false;
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;

        m_PlayerNetworkObject.positionInterpolation.Enabled = false;
        m_PlayerNetworkObject.positionChanged += player.WarpToFirstPosition;

        // Re-enable controller
        yield return new WaitForEndOfFrame();
        cc.enabled = true;

        // Respawn cooldown
        yield return new WaitForSeconds(10.0f);
        m_HealthHandler.Revive();
        m_PlayerMovement.ToggleDead();
    }

    private IEnumerator DamagedSequence()
    {
        m_PlayerMovement.ToggleDamaged();
        yield return new WaitForSeconds(3.0f);
        m_PlayerMovement.ToggleDamaged();
    }
}
