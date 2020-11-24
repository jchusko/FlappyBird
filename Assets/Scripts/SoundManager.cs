using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Enums;
using UnityEngine.UI;

public static class SoundManager
{
    public static void PlaySound(Sounds sound)
    {
        var gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sounds sound)
    {
        foreach(var clip in GameAssets.GetInstance().SoundArray)
        {
            if(clip.sound == sound) { return clip.audioClip; }
        }

        Debug.LogError("Sounds " + sound + " not found!");
        return null;
    }
}
