using System;
using FWGameLib.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Chronomance.Audio
{
    public class AudioSystem : Singleton<AudioSystem>
    {
        [Header("Control")]
        [SerializeField] private AudioMixer mixer;
        
        [Header("Audio Source Pooling")]
        [SerializeField] private PooledAudioSource audioSourcePrefab;
        [SerializeField] private int audioSourcePoolSize = 30;

        private ObjectPool<PooledAudioSource> _pool;

        // Play at origin
        // Null if sound is null
        [CanBeNull]
        public PooledAudioSource Play(SoundClipData clip)
        {
            return Play(clip, Vector3.zero);
        }

        [CanBeNull]
        public PooledAudioSource Play(SoundClipData clip, Vector3 position)
        {
            if (audioSourcePrefab == null || clip == null) return null;

            return _pool.Get().PlayClip(clip, position);
        }

        [CanBeNull]
        public PooledAudioSource Play(SoundClipData clip, Transform parent)
        {
            if (audioSourcePrefab == null || clip == null) return null;

            return _pool.Get().PlayClip(clip, parent);
        }

        private void Awake()
        {
            _pool = new ObjectPool<PooledAudioSource>(() =>
            {
                var pas = Instantiate(
                    audioSourcePrefab,
                    parent: transform,
                    true
                );
                pas.gameObject.SetActive(false);
                return pas;
            }, pas => 
            {
                pas.SetupForPlaying();
            }, pas =>
            {
                // Do nothing on release (handled by the object)
            }, pas =>
            {
                Destroy(pas.gameObject);
            }, false, audioSourcePoolSize);
        }

        private void OnEnable()
        {
            ChangeVolumeEvent.Handler += OnVolumeChange;
            ReleasePooledAudioSourceEvent.Handler += OnReleasePooledAudioSource;
        }

        private void OnDisable()
        {
            ChangeVolumeEvent.Handler -= OnVolumeChange;
            ReleasePooledAudioSourceEvent.Handler -= OnReleasePooledAudioSource;
        }

        private void OnReleasePooledAudioSource(ReleasePooledAudioSourceEvent e)
        {
            _pool.Release(e.Resource);
        }
        
        private void OnVolumeChange(ChangeVolumeEvent e)
        {
            string mixerFloatName = e.Type switch
            {
                ChangeVolumeEvent.MixerType.Master => "VolumeMaster",
                ChangeVolumeEvent.MixerType.Music => "VolumeMusic",
                ChangeVolumeEvent.MixerType.SoundEffects => "VolumeSoundEffects",
                _ => throw new ArgumentOutOfRangeException()
            };

            mixer.SetFloat(mixerFloatName, Mathf.Log10(e.Volume) * 20);
        }
    }
}