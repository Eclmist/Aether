using System.Text.RegularExpressions;
using UnityEngine;


/*
 * Was originally in javascript, which is why it's so janky
 */
public class PlantAudioAnimationEvent : MonoBehaviour
{
    [SerializeField] private AudioClip[] attack1; // Array of audio available

    private float attack1Level = 1.0f; // Volume Level

    [SerializeField] private AudioClip[] attack2; // Array of audio available

    private float attack2Level = 1.0f; // Volume Level
    private AudioClip[] castHit; // Array of audio available
    private float castHitLevel = 1.0f; // Volume Level
    private AudioClip[] castShoot; // Array of audio available
    private float castShootLevel = 1.0f; // Volume Level
    private AudioClip castWarmup;
    private float castWarmupLevel = 1.0f; // Volume Level
    private AudioClip[] death; // Array of audio available
    private float deathLevel = 1.0f; // Volume Level
        
    [SerializeField] 
    private AudioClip folliageLoop;
    private float folliageLoopLevel = 1.0f; // Volume Level
    private float folliageTimer = 0.0f;
    private AudioClip[] gotHit; // Array of audio available
    private float gotHitLevel = 1.0f; // Volume Level
    private AudioClip[] goToMonster; // Array of audio available
    private float goToMonsterLevel = 1.0f; // Volume Level
    private AudioClip[] goToPlant; // Array of audio available
    private float goToPlantLevel = 1.0f; // Volume Level
    private AudioClip[] idleBreak; // Array of audio available
    private float idleBreakLevel = 1.0f; // Volume Level
    private AudioClip[] jump; // Array of audio available
    private float jumpLevel = 1.0f; // Volume Level
    private AudioSource m_AudioSource;
    private AudioClip[] skip; // Array of audio available
    private float skipLevel = 1.0f; // Volume Level
    private float startAudioLevel;
    private AudioClip[] walkBack; // Array of audio available
    private float walkBackLevel = 1.0f; // Volume Level

    [SerializeField] private AudioClip[] walkForward; // Array of audio available

    private float walkForwardLevel = 1.0f; // Volume Level

    private void Start()
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
        return clips.Length != 0?clips[Random.Range(0, clips.Length)]: null;
    }

    /*
     * Callback function for animation events
     */
    public void PlayAudio(string audio)
    {
        switch (audio)
        {
            case "Attack1":
                Play(GetRandom(attack1));
                break;
            case "Attack2":
                Play(GetRandom(attack2));
                break;
            default:
                var split = Regex.Split(audio, "[0-9]");
                switch (split[0])
                {
                    case "Walk":
                        Play(GetRandom(walkForward));
                        break;
                }

                break;
        }
    }

    public void StartLoop(string audio)
    {
    }

    public void StopLoop(string audio)
    {
    }
}