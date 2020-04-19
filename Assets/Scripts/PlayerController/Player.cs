using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine.UI;

[RequireComponent(typeof(LocalNetworkTogglables))]
[RequireComponent(typeof(RevealActor))]
[RequireComponent(typeof(StealthActor))]
[RequireComponent(typeof(PlayerNetworkHandler))]
public class Player : PlayerBehavior, ICanInteract
{
    public System.Action PlayerDrop;
    public System.Action PlayerRespawn;

    [SerializeField]
    private GameObject m_Weapon;

    [SerializeField]
    private Text m_NameTag;

    [SerializeField]
    private bool m_IsEditorPlayer = false;

    private PlayerNetworkHandler m_PlayerNetworkHandler;
    private LocalNetworkTogglables m_LocalNetworkTogglables;
    private SkillHandler m_SkillHandler;
    private HealthHandler m_HealthHandler;

    private RevealActor m_RevealActor;
    private StealthActor m_StealthActor;

    private PlayerDetails m_PlayerDetails;
    private CharacterCustomizer m_CharacterCustomizer;
    private bool m_IsStealthy;

    private int m_CurrentActiveSkill;
    private void Awake()
    {
        m_LocalNetworkTogglables = GetComponent<LocalNetworkTogglables>();
        m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
        m_RevealActor = GetComponent<RevealActor>();
        m_StealthActor = GetComponent<StealthActor>();
        m_SkillHandler = GetComponent<SkillHandler>();
        m_HealthHandler = GetComponent<HealthHandler>();
        m_CharacterCustomizer = GetComponentInChildren<CharacterCustomizer>();
    }

    private void Start()
    {
        //AetherInput.GetPlayerActions().Stealth.performed += StealthInputCallback;

        if (m_IsEditorPlayer && AetherNetworkManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        // Set this in update because we're not sure when player details is set
        // TODO: Fix if continue in 4350
        if (m_PlayerDetails != null && m_NameTag != null)
            m_NameTag.text = m_PlayerDetails.GetName();
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;
    }

    private void OnTriggerEnter(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_ENTER);
    }

    private void OnTriggerStay(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_STAY);
    }

    private void OnTriggerExit(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>(), InteractionType.INTERACTION_TRIGGER_EXIT);
    }

    public SkillHandler GetSkillHandler()
    {
        return m_SkillHandler;
    }

    public HealthHandler GetHealthHandler()
    {
        return m_HealthHandler;
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

    public RevealActor GetRevealActor()
    {
        return m_RevealActor;
    }

    public void SetWeaponActive(bool weaponActive)
    {
        m_Weapon.SetActive(weaponActive);
    }

    public void UpdateToggleables()
    {
        m_LocalNetworkTogglables.UpdateOwner(networkObject.IsOwner);
    }

    // Toggles between stealth and reveal modes upon pressing stealth button.
    // Only the local player should be able to activate this.
    //private void StealthInputCallback(InputAction.CallbackContext ctx)
    //{
    //    // Ensure that only local player can call this
    //    if (networkObject == null || !networkObject.IsOwner)
    //        return;

    //    ButtonControl button = ctx.control as ButtonControl;
    //    if (!button.wasPressedThisFrame)
    //        return;


    //    m_IsStealthy = !m_IsStealthy;
    //    if (m_IsStealthy)
    //    {
    //        Debug.Log("Now in Stealth");
    //        m_RevealActor.enabled = false;
    //        m_StealthActor.enabled = true;
    //    }
    //    else
    //    {
    //        Debug.Log("Now revealing");
    //        m_RevealActor.enabled = true;
    //        m_StealthActor.enabled = false;
    //    }
    //}

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
    public void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
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
        m_CharacterCustomizer.SetDataPacked(details.GetCustomization());

        // Setup playermanager
        if (details.GetNetworkId() == networkObject.MyPlayerId)
            PlayerManager.Instance.SetLocalPlayer(this);

        PlayerManager.Instance.AddPlayer(this);
    }

    public void TriggerRespawn()
    {
        PlayerDrop?.Invoke();
        m_PlayerNetworkHandler.TriggerRespawn(PlayerRespawn);
    }

    public override void TriggerDamaged(RpcArgs args)
    {
        m_PlayerNetworkHandler.TriggerDamaged();
    }

    public override void TriggerJump(RpcArgs args)
    {
        m_PlayerNetworkHandler.TriggerJump();
    }

    public override void TriggerAttack(RpcArgs args)
    {
        m_PlayerNetworkHandler.TriggerAttack();
    }

    public override void TriggerDash(RpcArgs args)
    {
        m_PlayerNetworkHandler.TriggerDash();
    }

    public override void TriggerBackDash(RpcArgs args)
    {
        m_PlayerNetworkHandler.TriggerBackDash();
    }
}
