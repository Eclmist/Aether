using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(ClientServerTogglables))]
[RequireComponent(typeof(RevealActor))]
[RequireComponent(typeof(StealthActor))]
public class Player : PlayerBehavior, ICanInteract
{
    private bool m_IsStealthy;

    private PlayerMovement m_PlayerMovement;
    private PlayerAnimation m_PlayerAnimation;
    private ClientServerTogglables m_ClientServerTogglables;

    private RevealActor m_RevealActor;
    private StealthActor m_StealthActor;

    private void Awake()
    {
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
    }

    private void WarpToFirstPosition(Vector3 field, ulong timestep)
    {
        networkObject.positionChanged -= WarpToFirstPosition;
        networkObject.positionInterpolation.Enabled = true;
        networkObject.positionInterpolation.current = networkObject.position;
        networkObject.positionInterpolation.target = networkObject.position;
    }
      
    public PlayerMovement GetPlayerMovement()
    {
        return m_PlayerMovement;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        m_PlayerMovement = playerMovement;
    }

    public bool IsStealthy()
    {
        return m_IsStealthy;
    }

    private void HandleStealth(InputAction.CallbackContext ctx)
    {
        Debug.Log("Switch reveal mode");
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;
        
        m_IsStealthy = !m_IsStealthy;
        if (m_IsStealthy)
        {
            m_RevealActor.enabled = false;
            m_StealthActor.enabled = true;
        }
        else
        {
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
