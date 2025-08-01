using UnityEngine;

namespace Chronomance.Audio
{
    /// <summary>
    /// An instance of a sound clip playing
    /// </summary>
    public class SoundClipInstance
    {
        private readonly SoundClipData _data;
        private int _currentClipIndex;

        public SoundClipInstance(SoundClipData data)
        {
            _data = data;
        }

        public AudioClip NextClip()
        {
            if (_data.clips.Length == 0)
            {
                Debug.LogWarning("No audio clips available in SoundClipData!");
                return null;
            }

            switch (_data.pickClipStrategy)
            {
                case PickAudioClipStrategy.Random:
                    _currentClipIndex = Random.Range(0, _data.clips.Length);
                    break;
                case PickAudioClipStrategy.Sequential:
                    _currentClipIndex = (_currentClipIndex + 1) % _data.clips.Length;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            return _data.clips[_currentClipIndex];
        }
    }
}