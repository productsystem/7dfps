using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play(string n)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == n);
        if(s == null)
            return;
        s.source.Play();
    }
}
