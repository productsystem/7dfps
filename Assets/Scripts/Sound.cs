using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string soundName;
    public float volume;

    [HideInInspector]
    public AudioSource source;

}