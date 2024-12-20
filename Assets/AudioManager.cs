using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
    public AudioSource audioSource; // Fonte de áudio principal


    private void Awake()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

}


[Serializable]
public class Sound
{
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume = 1f;
}


public static class SoundUtils
{
    public static void PlayShot(this AudioSource source, Sound sound)
    {
        source.PlayOneShot(sound.audioClip, sound.volume);
    }

    public static void PlayRandomShot(this AudioSource source, Sound[] sounds)
    {
        var sound = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        source.PlayOneShot(sound.audioClip, sound.volume);
    }
}
