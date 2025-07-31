using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioMixer))]
public class SoundMixer : MonoBehaviour
{
    [SerializeField] private MixerType mixerType;

    private AudioMixer _mixer;

    private void Awake()
    {
        _mixer = GetComponent<AudioMixer>();
    }

    private void OnEnable()
    {
        ChangeVolumeEvent.Handler += OnVolumeChange;
    }

    private void OnDisable()
    {
        ChangeVolumeEvent.Handler -= OnVolumeChange;
    }

    private void OnVolumeChange(ChangeVolumeEvent e)
    {
        if (e.MixerType != mixerType) return;
        
        // TODO: set mixer volume
    }
}