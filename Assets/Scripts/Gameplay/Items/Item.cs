using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SkillItem m_ItemSkill;

    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (interactor != null && interactor is Player player)
        {
            switch (interactionType)
            {
                case InteractionType.INTERACTION_TRIGGER_ENTER:
                    PlayPickUpSound();
                    HandleItemSkill(player);
                    // E4 Hack
                    gameObject.SetActive(false);
                    // Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
    private void PlayPickUpSound()
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 3.0f, 1.2f);
    }

    public void HandleItemSkill(Player player)
    {
        if (player == PlayerManager.Instance.GetLocalPlayer())
            player.GetSkillHandler().AddSkill(m_ItemSkill); //must be after or else will be deleted
    }
}
