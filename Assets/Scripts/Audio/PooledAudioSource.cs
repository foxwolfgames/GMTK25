using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioLowPassFilter))]
public class PooledAudioSource : MonoBehaviour
{
    private Guid _id;
    private AudioSource _source;
    private AudioLowPassFilter _lowPassFilter;
    private Transform _parentTransform;
    private SoundClipInstance _currentSound;
    
    private bool _isPlaying;
    private bool _isPausedByGame;
    
    public Guid Id => _id;

    public PooledAudioSource PlayClip(SoundClipData clip, Vector3 position)
    {
        SetupForPlaying();
        transform.position = position;
        return Play(clip);
    }

    public PooledAudioSource PlayClip(SoundClipData clip, Transform parent)
    {
        SetupForPlaying();
        _parentTransform = parent;
        transform.position = parent.position;
        return Play(clip);
    }

    private void Awake()
    {
        _id = Guid.NewGuid();
        _source = GetComponent<AudioSource>();
        _lowPassFilter = GetComponent<AudioLowPassFilter>();
    }

    private void OnEnable()
    {
        PauseAudioEvent.Handler += OnPause;
        UnpauseAudioEvent.Handler += OnUnpause;
    }

    private void OnDisable()
    {
        PauseAudioEvent.Handler -= OnPause;
        UnpauseAudioEvent.Handler -= OnUnpause;
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
        }
    }

    private void SetupForPlaying()
    {
        // Unexpected: We should not set up for play state if already playing
        if (gameObject.activeInHierarchy)
        {
            throw new Exception("Attempted to setup to play pooled audio source that is already active.");
        }
        
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

        _source.Play();
        _isPlaying = true;
        return this;
    }

    private void OnPause(PauseAudioEvent _)
    {
        // TODO
    }

    private void OnUnpause(UnpauseAudioEvent _)
    {
        // TODO
    }
}