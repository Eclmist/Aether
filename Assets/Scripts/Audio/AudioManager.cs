using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEntry
    {
        public string m_Identifier;
        public AudioClip m_Clip;

        [Range(0.0f, 3.0f)]
        public float m_Volume = 1;
        [Range(0.1f, 3.0f)]
        public float m_Pitch = 1;

        [HideInInspector]
        public AudioSource m_Source;
    }

    [SerializeField]
    private AudioMixer m_MasterMixer;

    [SerializeField]
    private SoundEntry[] m_SfxLibrary;

    // Singleton
    public static AudioManager m_Instance;

    void Awake()
    {
        if (m_Instance != null)
            Destroy(this);

        m_Instance = this;

        foreach(SoundEntry s in m_SfxLibrary)
        {
            GameObject soundObject = new GameObject(s.m_Identifier + " source");
            soundObject.transform.parent = transform;
            s.m_Source = soundObject.AddComponent<AudioSource>();
            s.m_Source.outputAudioMixerGroup = m_MasterMixer.FindMatchingGroups("SFX")[0];
            s.m_Source.clip = s.m_Clip;
            s.m_Source.volume = s.m_Volume;
            s.m_Source.pitch = s.m_Pitch;
        }
    }

    public void PlaySoundAtPosition(string identifier, Vector3 position, float volume = 1, float pitch = 1)
    {
        SoundEntry s = Array.Find(m_SfxLibrary, sound => sound.m_Identifier == identifier);

        if (s == null)
        {
            Debug.LogWarning("The requested sound \"" + identifier + "\" does not exist!");
            return;
        }

        // GameObject tempSoundPlayer = Instantiate(s.m_Source.gameObject);
        // tempSoundPlayer.transform.position = position;
        // AudioSource audioSource = tempSoundPlayer.GetComponent<AudioSource>();
        // audioSource.volume *= volume;
        // audioSource.pitch *= pitch;

        // audioSource.Play();
        // Destroy(tempSoundPlayer, s.m_Clip.length);
    }
}
