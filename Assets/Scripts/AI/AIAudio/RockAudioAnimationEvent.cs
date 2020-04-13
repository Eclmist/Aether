using System.Text.RegularExpressions;
using UnityEngine;


/*
 * Was originally in javascript, which is why it's so janky
 */
public class RockAudioAnimationEvent : MonsterAudio
{

    /*
     * Callback function for animation events
     */
    public override void PlayAudio(string audio)
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
            case "Got Hit":
                Debug.Log(audio);
                Play(GetRandom(gotHit));
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

    public void LeftFoot(string audio)
    {
        PlayAudio(audio);
    }
    
    public void RightFoot(string audio)
    {
        PlayAudio(audio);
    }
    
    public void StartLoop()
    {
    }

    public void StopLoop()
    {
    }
}