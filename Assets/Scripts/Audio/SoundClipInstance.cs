using UnityEngine;

namespace Chronomance.Audio
{
    /// <summary>
    /// An instance of a sound clip playing
    /// </summary>
    public class SoundClipInstance
    {
        public SoundClipData Data { get; }
        private int _currentClipIndex;

        public SoundClipInstance(SoundClipData data)
        {
            Data = data;
        }

        public AudioClip NextClip()
        {
            if (Data.clips.Length == 0)
            {
                Debug.LogWarning("No audio clips available in SoundClipData!");
                return null;
            }

            _currentClipIndex = Data.pickClipStrategy switch
            {
                PickAudioClipStrategy.Random => Random.Range(0, Data.clips.Length),
                PickAudioClipStrategy.Sequential => (_currentClipIndex + 1) % Data.clips.Length,
                _ => throw new System.ArgumentOutOfRangeException()
            };

            return Data.clips[_currentClipIndex];
        }
    }
}