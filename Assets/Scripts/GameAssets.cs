    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Enums;


public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    public Sprite PipeHeadSprite;
    public Transform PFPipeHead;
    public Transform PFPipeBody;
    public Transform PFGround;
    public Transform PFCloud_1;
    public Transform PFCloud_2;
    public Transform PFCloud_3;

    public SoundAudioClip[] SoundArray;

    [Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;
    }
}
