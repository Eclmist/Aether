using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ItemSkill m_ItemSkill;
    public void Interact(ICanInteract interactor, InteractionType interactionType)
    {
        if (interactor != null && interactor is Player player)
        {
            switch (interactionType)
            {
                case InteractionType.INTERACTION_TRIGGER_ENTER:
                    PlayPickUpSound();
                    HandleItemSkill(player);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
    
    private void PlayPickUpSound()
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public void HandleItemSkill(Player player)
    {

        Debug.Log(m_ItemSkill);

        ItemSkill itemSkill = player.GetSkillsTransform().gameObject.AddComponent(m_ItemSkill.GetType()) as ItemSkill; //adds child type
        itemSkill.InitializeSkill();
        player.GetComponentInChildren<SkillHandler>().AddSkill(itemSkill); //must be after or else will be deleted

    }
}
