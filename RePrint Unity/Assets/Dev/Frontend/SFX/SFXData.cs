using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "Scriptable Objects/SFXData")]
public class SFXData : ScriptableObject
{
    [field: SerializeField] public AudioClip[] Clips { get; private set; }
    [field: SerializeField] public AudioChannel Channel { get; private set; }
    [field: SerializeField, Range(0.1f, 2)] public float PitchMin { get; private set; } = 1;
    [field: SerializeField, Range(0.1f, 2)] public float PitchMax { get; private set; } = 1;
    [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 1;
    [field: SerializeField, HideInInspector] public bool OverwriteIfAlreadyPlaying { get; private set; }

    [System.NonSerialized, HideInInspector] public List<int> clipIndexPool;
    [System.NonSerialized, HideInInspector] public int lastClipIndexPlayed = -1;

    public AudioClip GetClip()
    {
        if (Clips.Length == 1)
        {
            return Clips[0];
        }

        if (clipIndexPool == null)
        {
            clipIndexPool = new List<int>();
        }

        if (clipIndexPool.Count == 0)
        {
            for (int i = 0; i < Clips.Length; i++)
            {
                if (i != lastClipIndexPlayed)
                    clipIndexPool.Add(i);
            }
        }

        int clipToPlay = clipIndexPool[Random.Range(0, clipIndexPool.Count)];
        clipIndexPool.Remove(clipToPlay);
        lastClipIndexPlayed = clipToPlay;
        return Clips[clipToPlay];
    }
}
