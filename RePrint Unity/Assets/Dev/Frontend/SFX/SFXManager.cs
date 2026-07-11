using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioChannel
{
    SoundEffects,
    Music,
    Voice,
    Ambience,
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager sfx;


    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixer soundEffectsMixer;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer voiceMixer;
    [SerializeField] private AudioMixer ambienceMixer;

    [SerializeField] private List<SFXData> cachedAudio;

    private Dictionary<SFXData, AudioSource> sources;

    void Awake()
    {
        if (sfx != null && sfx != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            sfx = this;
            DontDestroyOnLoad(gameObject);
        }

        CreateAudioSources();
    }

    public void CreateAudioSources()
    {
        sources = new Dictionary<SFXData, AudioSource>(10);

        if (cachedAudio == null)
            return;

        foreach (SFXData sfx in cachedAudio)
        {
            if (!sources.ContainsKey(sfx))
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                sources.Add(sfx, newSource);
                newSource.playOnAwake = false;
                switch (sfx.Channel)
                {
                    case AudioChannel.SoundEffects:
                        newSource.outputAudioMixerGroup = soundEffectsMixer.outputAudioMixerGroup;
                        break;
                    case AudioChannel.Music:
                        newSource.outputAudioMixerGroup = musicMixer.outputAudioMixerGroup;
                        break;
                    case AudioChannel.Voice:
                        newSource.outputAudioMixerGroup = voiceMixer.outputAudioMixerGroup;
                        break;
                    case AudioChannel.Ambience:
                        newSource.outputAudioMixerGroup = ambienceMixer.outputAudioMixerGroup;
                        break;
                }
            }
        }
    }

    public void Play(SFXData sfx)
    {
        if (!sources.ContainsKey(sfx))
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            sources.Add(sfx, newSource);
        }

        AudioSource source = sources[sfx];

        if (sfx.OverwriteIfAlreadyPlaying && source.isPlaying)
        {
            source.Stop();
        }

        source.clip = sfx.GetClip();
        source.volume = sfx.Volume;
        source.pitch = UnityEngine.Random.Range(sfx.PitchMin, sfx.PitchMax);
        source.Play();
    }
}
