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
            s.source.loop = s.isLoopable;
        }
    }

    public void Play(string n)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == n);
        if(s == null)
            return;
        s.source.Play();
    }

    public bool IsPlaying(string n)
    {
        Sound sound = Array.Find(sounds, s => s.soundName == n);
        if (sound != null)
        {
            return sound.source.isPlaying;
        }
        return false;
    }

    public void Stop(string n)
    {
        Sound sound = Array.Find(sounds, s => s.soundName == n);
        if (sound != null && sound.source.isPlaying)
        {
            sound.source.Stop();
        }
    }
}
