using System;
using UnityEngine;

namespace XG.Studios
{
    [Serializable]
    public class Sound
    {
        public enum AudioTypes
        {
            Music,
            SoundEffects
        }

        public AudioTypes audioType;

        [HideInInspector]
        public AudioSource source;
        public string clipName;
        public AudioClip audioClip;
        public bool isLoop;
        public bool playOnAwake;

        [Range(0, 1)]
        public float volume = 0.15f;
    }
}
