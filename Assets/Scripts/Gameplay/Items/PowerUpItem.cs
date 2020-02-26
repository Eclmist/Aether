﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpItem : MonoBehaviour, IInteractable
{
    [SerializeField]
    private PowerUpBase m_PowerUp;
    public void Interact(ICanInteract interactor)
    {
        if (interactor != null && interactor is Player player)
        {
            PlayPickUpSound();
            HandlePowerUp(player);
            Destroy(gameObject);
        }
    }
    
    private void PlayPickUpSound()
    {
        AudioManager.m_Instance.PlaySound("MAGIC_Powerup", 1.0f, 1.2f);
    }

    public void HandlePowerUp(Player player)
    {
        PowerUpBase powerUp = player.gameObject.AddComponent(m_PowerUp.GetType()) as PowerUpBase; //adds child type
        powerUp.InitializePowerUp();
    }
}
