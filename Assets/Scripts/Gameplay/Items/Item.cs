using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ItemSkill m_ItemSkill;
    public void Interact(ICanInteract interactor)
    {
        if (interactor != null && interactor is Player player)
        {
            PlayPickUpSound();
            HandleItemSkill(player);
            Destroy(gameObject);
        }
    }
    
    private void PlayPickUpSound()
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public void HandleItemSkill(Player player)
    {
        Transform skillObject = player.transform.Find("Skills");
        
        if (skillObject == null)
        {
            Debug.LogError("Player should have a child GameObject named Skills");
            return;
        }
        
        Debug.Log("Added");
        ItemSkill itemSkill = skillObject.gameObject.AddComponent(m_ItemSkill.GetType()) as ItemSkill; //adds child type
        itemSkill.InitializeSkill();
        player.GetComponentInChildren<SkillHandler>().AddSkill(itemSkill); //must be after or else will be deleted

    }
}
