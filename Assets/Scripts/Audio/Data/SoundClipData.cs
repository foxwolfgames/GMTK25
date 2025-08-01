using UnityEngine;
using UnityEngine.Audio;

namespace Chronomance.Audio
{
    [CreateAssetMenu(fileName = "NewSoundClip", menuName = "Audio/Sound Clip")]
    public class SoundClipData : ScriptableObject
    {
        [Header("Information")]
        public AudioClip[] clips;
        public AudioMixerGroup output;

        [Header("Control")]
        public bool ignorePause;
        public PickAudioClipStrategy pickClipStrategy;
    }
}