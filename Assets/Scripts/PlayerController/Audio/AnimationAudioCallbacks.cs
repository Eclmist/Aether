using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimationAudioCallbacks : MonoBehaviour
{
    public void PlayFootstepSound(float volumeModifier)
    {
        AudioManager.m_Instance.PlaySoundAtPosition("CHR_Footstep_Grass", transform.position,
            Random.Range(0.8f, 1.2f) * volumeModifier, Random.Range(0.95f, 1.05f));
    }
}
