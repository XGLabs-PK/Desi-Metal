using System;
using UnityEngine;
using UnityEngine.Audio;
using XGStudios;

namespace XG.Studios
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        AudioMixerGroup musicMixerGroup;
        [SerializeField]
        AudioMixerGroup soundEffectsMixerGroup;
        [SerializeField]
        Sound[] sounds;

        void Awake()
        {
            Instance = this;

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioClip;
                s.source.loop = s.isLoop;
                s.source.volume = s.volume;

                switch (s.audioType)
                {
                    case Sound.AudioTypes.SoundEffects:
                        s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                        break;
                    case Sound.AudioTypes.Music:
                        s.source.outputAudioMixerGroup = musicMixerGroup;
                        break;
                }

                if (s.playOnAwake)
                    s.source.Play();
            }
        }

        public void Play(string clipName)
        {
            Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipName);

            if (s == null)
            {
                Debug.LogError("Sound: " + clipName + " not found!");
                return;
            }

            s.source.Play();
        }

        public void Stop(string clipName)
        {
            Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipName);

            if (s == null)
            {
                Debug.LogError("Sound: " + clipName + " not found!");
                return;
            }

            s.source.Stop();
        }

        public void UpdateMixerVolume()
        {
            musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(UIStuff.MusicVolume) * 20);

            soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume",
                Mathf.Log10(UIStuff.SoundEffectsVolume) * 20);
        }
    }
}
