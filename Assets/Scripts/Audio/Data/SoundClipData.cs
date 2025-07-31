using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundClip", menuName = "Audio/Sound Clip")]
public class SoundClipData : ScriptableObject
{
    [Header("Information")]
    public MixerType mixerType;
    public AudioClip[] clips;

    [Header("Control")]
    public bool ignorePause;
    public PickAudioClipStrategy pickClipStrategy;
}