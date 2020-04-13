
using System.Text.RegularExpressions;
using UnityEngine;


/*
 * Was originally in javascript, which is why it's so janky
 */
public class MonsterAudio : MonoBehaviour
{
    [SerializeField] protected AudioClip[] attack1; // Array of audio available
    [SerializeField] protected AudioClip[] attack2; // Array of audio available

    protected float attack2Level = 1.0f; // Volume Level
    protected AudioClip[] castHit; // Array of audio available
    protected float castHitLevel = 1.0f; // Volume Level
    protected AudioClip[] castShoot; // Array of audio available
    protected float castShootLevel = 1.0f; // Volume Level
    protected AudioClip castWarmup;
    protected float castWarmupLevel = 1.0f; // Volume Level

    [SerializeField] 
    protected AudioClip[] death; // Array of audio available
    
    [SerializeField] 
    protected AudioClip folliageLoop;
    protected float folliageLoopLevel = 1.0f; // Volume Level
    protected float folliageTimer = 0.0f;

    [SerializeField] 
    protected AudioClip[] gotHit; // Array of audio available
    protected AudioClip[] goToMonster; // Array of audio available
    protected float goToMonsterLevel = 1.0f; // Volume Level
    protected AudioClip[] goToPlant; // Array of audio available
    protected float goToPlantLevel = 1.0f; // Volume Level
    protected AudioClip[] idleBreak; // Array of audio available
    protected float idleBreakLevel = 1.0f; // Volume Level
    protected AudioClip[] jump; // Array of audio available
    protected float jumpLevel = 1.0f; // Volume Level
    protected AudioSource m_AudioSource;
    protected AudioClip[] skip; // Array of audio available
    protected float skipLevel = 1.0f; // Volume Level
    protected float startAudioLevel;
    protected AudioClip[] walkBack; // Array of audio available
    protected float walkBackLevel = 1.0f; // Volume Level

    [SerializeField] protected AudioClip[] walkForward; // Array of audio available

    protected float walkForwardLevel = 1.0f; // Volume Level

    protected void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected void Play(AudioClip clip)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.Play();
    }

    protected AudioClip GetRandom(AudioClip[] clips)
    {
        return clips.Length != 0?clips[Random.Range(0, clips.Length)]: null;
    }

    /*
     * Callback function for animation events
     */
    public virtual void PlayAudio(string audio)
    {
        switch (audio)
        {
            case "Attack1":
                Play(GetRandom(attack1));
                break;
            case "Attack2":
                Play(GetRandom(attack2));
                break;
            case "Death":
                Play(GetRandom(death));
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