using System;
using UnityEngine;

namespace Chronomance.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour
    {
        private Guid _id;
        private AudioSource _source;
        private Transform _parentTransform;
        private SoundClipInstance _currentSound;

        private bool _isPlaying;
        private bool _isPausedByGame;

        public Guid Id => _id;

        public PooledAudioSource PlayClip(SoundClipData clip, Vector3 position)
        {
            transform.position = position;
            return Play(clip);
        }

        public PooledAudioSource PlayClip(SoundClipData clip, Transform parent)
        {
            _parentTransform = parent;
            transform.position = parent.position;
            return Play(clip);
        }

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            PauseAudioEvent.Handler += OnPause;
            UnpauseAudioEvent.Handler += OnUnpause;
            StopAudioSourceEvent.Handler += OnStopAudioSource;
        }

        private void OnDisable()
        {
            PauseAudioEvent.Handler -= OnPause;
            UnpauseAudioEvent.Handler -= OnUnpause;
            StopAudioSourceEvent.Handler -= OnStopAudioSource;
        }

        private void FixedUpdate()
        {
            if (_parentTransform != null)
            {
                transform.position = _parentTransform.position;
            }
        }

        private void Update()
        {
            // Check if the audio has finished playing
            if (_isPlaying && !_source.isPlaying && !_isPausedByGame)
            {
                _isPlaying = false;
                _parentTransform = null;
                _currentSound = null;
                gameObject.SetActive(false); // Deactivate this pooled audio source
                new ReleasePooledAudioSourceEvent(this).Invoke();
            }
        }

        // Called when this is pooled audio source is get by the pool
        public void SetupForPlaying()
        {
            // Unexpected: We should not set up for play state if already playing
            if (gameObject.activeInHierarchy)
            {
                throw new Exception("Attempted to setup to play pooled audio source that is already active.");
            }

            // New id to track by
            _id = Guid.NewGuid();
            gameObject.SetActive(true);
            _isPausedByGame = false;
            transform.localPosition = Vector3.zero; // Reset local position
        }

        private PooledAudioSource Play(SoundClipData clip)
        {
            var clipInstance = new SoundClipInstance(clip);
            _currentSound = clipInstance;
            _source.clip = clipInstance.NextClip();
            _source.ignoreListenerPause = clip.ignorePause;

            _source.outputAudioMixerGroup = clip.output;
            _source.Play();
            _isPlaying = true;
            return this;
        }

        private void OnPause(PauseAudioEvent _)
        {
            if (!_isPlaying) return;
            if (_currentSound?.Data.ignorePause ?? true) return;
            
            _isPausedByGame = true;
            _source.Pause();
        }

        private void OnUnpause(UnpauseAudioEvent _)
        {
            if (!_isPlaying) return;
            if (_currentSound?.Data.ignorePause ?? true) return;
            
            _isPausedByGame = false;
            _source.UnPause();
        }

        private void OnStopAudioSource(StopAudioSourceEvent e)
        {
            if (!_isPlaying) return;
            
            // Only listen to events specifically targeting this audio source
            if (e.AudioSourceId != _id) return;

            // Stop sound, Update will take care of deactivation
            _source.Stop();
        }
    }
}