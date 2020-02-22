using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(ClientServerTogglables))]
[RequireComponent(typeof(RevealActor))]
[RequireComponent(typeof(StealthActor))]
public class Player : PlayerBehavior, ICanInteract
{
    // Events for player manager to hook onto
    public delegate void PlayerEvent(Player player);
    public event PlayerEvent playerLoaded;

    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;
    private ClientServerTogglables m_ClientServerTogglables;

    private RevealActor m_RevealActor;
    private StealthActor m_StealthActor;

    private int m_TeamId;
    private bool m_IsStealthy;

    private void Awake()
    {
        // Cannot use RequireComponent for movement and animation
        // (both scripts cannot be deleted for networkplayer otherwise)
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        m_ClientServerTogglables = GetComponent<ClientServerTogglables>();
        m_RevealActor = GetComponent<RevealActor>();
        m_StealthActor = GetComponent<StealthActor>();

        m_StealthActor.enabled = false;
    }

    private void Start()
    {
        AetherInput.GetPlayerActions().Stealth.performed += HandleStealth;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        m_ClientServerTogglables.UpdateOwner(networkObject.IsOwner);
        networkObject.positionInterpolation.Enabled = false;
        networkObject.positionChanged += WarpToFirstPosition;

        if (networkObject.IsOwner)
            playerLoaded(this);
    }

    // Workaround for player sliding into position when instantiated.
    private void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }

    public int GetTeam()
    {
        return m_TeamId;
    }

    public void SetTeam(int teamId)
    {
        m_TeamId = teamId;
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

    // Toggles between stealth and reveal modes upon pressing stealth button.
    // Only the local player should be able to activate this.
    private void HandleStealth(InputAction.CallbackContext ctx)
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

    private void InteractWith(IInteractable interactable)
    {
        if (interactable != null) // null check done here instead. 
        {
            interactable.Interact(this);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        InteractWith(c.GetComponent<IInteractable>());
    }
}
