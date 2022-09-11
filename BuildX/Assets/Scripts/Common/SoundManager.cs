using System;
using System.Linq;
using UnityEngine;

public enum AudioType
{
    None,
    Click,
    CardHold,
    GotChips,
    NoMatch,
    Rewards,
    Bingo,
    RoundOver,
}

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class AudioItem
    {
        [SerializeField] private AudioType type;
        [SerializeField] private AudioClip clip;

        public AudioType Type => type;

        public AudioClip Clip => clip;
    }

    private static SoundManager _instance;

    [SerializeField] private AudioSource[] sfxSource;
    [SerializeField] private AudioItem[] items;

    private AudioSource SfxSource
    {
        get
        {
            foreach (var item in sfxSource)
            {
                if (!item.isPlaying)
                    return item;
            }

            return sfxSource[0];
        }
    }

    private AudioType _audio;

    private AudioType Audio
    {
        get => _audio;
        set
        {
            _audio = value;
            if (value == AudioType.None) return;
            var item = items.FirstOrDefault(t => t.Type == value);
            var source = SfxSource;
            if (source == null || item == null) return;
            source.clip = item.Clip;
            source.Play();
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public static void Play(AudioType audioType)
    {
        _instance.Audio = audioType;
    }
}