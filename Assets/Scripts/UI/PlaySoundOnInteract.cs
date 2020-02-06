using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class PlaySoundOnInteract : MonoBehaviour
{
    public string m_SelectSoundId = "UI_Hover";
    public string m_ClickSoundId = "GEN_Success_1";

    public void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        
        // On select trigger
        EventTrigger.Entry selectTrigger = new EventTrigger.Entry();
        selectTrigger.eventID = EventTriggerType.Select;
        selectTrigger.callback.AddListener((eventData) => { PlaySelectSound(); });
        eventTrigger.triggers.Add(selectTrigger);

        // On click trigger
        EventTrigger.Entry clickTrigger = new EventTrigger.Entry();
        clickTrigger.eventID = EventTriggerType.Submit;
        clickTrigger.callback.AddListener((eventData) => { PlayClickSound(); });
        eventTrigger.triggers.Add(clickTrigger);
    }

    public void PlaySelectSound()
    {
        AudioManager.m_Instance.PlaySound(m_SelectSoundId, 0.5f, 1.5f);
    }

    public void PlayClickSound()
    {
        AudioManager.m_Instance.PlaySound(m_ClickSoundId, 0.5f, 2.5f);
    }
}
