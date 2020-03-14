using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(ClientServerTogglables))]
[RequireComponent(typeof(PlayerStance))]
[RequireComponent(typeof(RevealActor))]
[RequireComponent(typeof(StealthActor))]
public class Player : PlayerBehavior, ICanInteract
{
    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;
    private PlayerNetworkAnimation m_PlayerNetworkAnimation;
    private ClientServerTogglables m_ClientServerTogglables;
    private Transform m_SkillsTransform;

    private RevealActor m_RevealActor;
    private StealthActor m_StealthActor;

    private PlayerDetails m_PlayerDetails;
    private bool m_IsStealthy;

    // TODO remove this once health system has been merged. 
    private double m_health = 100;
    public void DamageHealth(double damage)
    {
        m_health -= damage;
    }


    private void Awake()
    {
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_PlayerNetworkAnimation = GetComponent<PlayerNetworkAnimation>();
        m_RevealActor = GetComponent<RevealActor>();
        m_StealthActor = GetComponent<StealthActor>();
        
        m_RevealActor.enabled = false;
        m_StealthActor.enabled = false;
        
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
        m_SkillsTransform = new GameObject("Skills").transform;
        m_SkillsTransform.gameObject.AddComponent<SkillHandler>();
        m_SkillsTransform.parent = gameObject.transform;
    }

    public Transform GetSkillsTransform()
    {
        return m_SkillsTransform;
    }
        

    private void Start()
    {
        AetherInput.GetPlayerActions().Stealth.performed += StealthInputCallback;

    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;
    }

    private void Update()
    {
        Shader.SetGlobalVector("_LocalPlayerPosition", transform.position);
    }

    private void OnTriggerEnter(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_ENTER);
    }

    private void OnTriggerExit(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_EXIT);
    }

    public PlayerDetails GetPlayerDetails()
    {
        return m_PlayerDetails;
    }

    public void SetDetails(PlayerDetails details)
    {
        if (details == null)
            Debug.Log("Details should not be null");

        m_PlayerDetails = details;
    }
      
    public PlayerMovement GetPlayerMovement()
    {
        return m_PlayerMovement;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        m_PlayerMovement = playerMovement;
    }

    public RevealActor GetRevealActor()
    {
        return m_RevealActor;
    }

    public void UpdateToggleables()
    {
        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
    }

    // Toggles between stealth and reveal modes upon pressing stealth button.
    // Only the local player should be able to activate this.
    private void StealthInputCallback(InputAction.CallbackContext ctx)
    {
        // Ensure that only local player can call this
        if (networkObject == null || !networkObject.IsOwner)
            return;

        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        
        m_IsStealthy = !m_IsStealthy;
        if (m_IsStealthy)
        {
            Debug.Log("Now in Stealth");
            m_RevealActor.enabled = false;
            m_StealthActor.enabled = true;
        }
        else
        {
            Debug.Log("Now revealing");
            m_RevealActor.enabled = true;
            m_StealthActor.enabled = false;
        }
    }

    private void InteractWith(IInteractable interactable, InteractionType interactionType)
    {
        if (interactable != null) // null check done here instead. 
            interactable.Interact(this, interactionType);
    }

    ////////////////////
    ///
    /// NETWORK CODE
    ///
    ////////////////////

    // Workaround for player sliding into position when instantiated.
    private void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }

    public override void TriggerJump(RpcArgs args)
    {
        m_PlayerNetworkAnimation.TriggerJump();
    }

    // RPC sent by host to send player details to all clients
    public override void TriggerUpdateDetails(RpcArgs args)
    {
        // May not be run on main thread for the sake of performance.
        // Nothing else should be touching player details at this point anyway.

        PlayerDetails details = PlayerDetails.FromArray(args.Args);
        if (details == null)
            Debug.LogWarning("Details not received correctly");
        m_PlayerDetails = details;

        // Setup playermanager
        if (details.GetNetworkId() == networkObject.MyPlayerId)
            PlayerManager.Instance.SetLocalPlayer(this);

        PlayerManager.Instance.AddPlayer(this);
    }
}
