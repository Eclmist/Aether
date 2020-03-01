using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class PlantMonsterAnimationEvent : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] attack1;  // Array of audio available
	private float attack1Level = 1.0f; // Volume Level
	[SerializeField]
	private AudioClip[] attack2; // Array of audio available
	private float attack2Level  = 1.0f; // Volume Level
	private AudioClip castWarmup ;
	private float castWarmupLevel = 1.0f; // Volume Level
	private AudioClip[] castShoot  ; // Array of audio available
	private float castShootLevel = 1.0f; // Volume Level
	private AudioClip[] castHit  ; // Array of audio available
	private float castHitLevel  = 1.0f; // Volume Level
	private AudioClip[] goToMonster ; // Array of audio available
	private float goToMonsterLevel = 1.0f; // Volume Level
	private AudioClip[] death ;									// Array of audio available
	private float deathLevel						= 1.0f;						// Volume Level
	private AudioClip folliageLoop	 ;
	private float folliageLoopLevel			= 1.0f;						// Volume Level
	private AudioClip[] gotHit				 ;									// Array of audio available
	private float gotHitLevel				= 1.0f;						// Volume Level
	private AudioClip[] idleBreak			 ;									// Array of audio available
	private float idleBreakLevel			= 1.0f;						// Volume Level
	private AudioClip[] jump				 ;									// Array of audio available
	private float jumpLevel						= 1.0f;						// Volume Level
	private AudioClip[] goToPlant			 ;									// Array of audio available
	private float goToPlantLevel				= 1.0f;						// Volume Level
	private AudioClip[] skip				 ;									// Array of audio available
	private float skipLevel						= 1.0f;						// Volume Level
	private AudioClip[] walkBack			 ;									// Array of audio available
	private float walkBackLevel				= 1.0f;						// Volume Level
	[SerializeField]
	private AudioClip[] walkForward			 ;									// Array of audio available
	private float walkForwardLevel				= 1.0f;						// Volume Level
	private float startAudioLevel		;
	
	[SerializeField]
	private float desiredAudioLevel	= 0.0f;
	float folliageTimer				= 0.0f;
	private AudioSource m_AudioSource;
	
	void Start () 
	{
		m_AudioSource = GetComponent<AudioSource>();
	}

	private void Play(AudioClip clip)
	{
		m_AudioSource.clip = clip;
		m_AudioSource.Play();
	}

	private AudioClip GetRandom(AudioClip[] clips)
	{
		return clips?[Random.Range(0, clips.Length)];
	}

	/*
	 * Callback function for animation events
	 */
	public void PlayAudio(String audio)
	{
		switch (audio)
		{
			case "Attack1":
				Play(GetRandom(attack1));
				break;
			case "Attack2":
				Play(attack2[0]);
				break;
			default:	
				string[] split = Regex.Split(audio, "[0-9]");
				switch (split[0])
				{
					case "Walk":
						Play(walkForward[0]);
						break;
				}

				break;
		}
	}

    public void StartLoop()
    {
    }

    public void StopLoop()
    {
    }
}
