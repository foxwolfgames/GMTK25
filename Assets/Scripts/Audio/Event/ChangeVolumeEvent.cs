using FWGameLib.Common.EventSystem;

public class ChangeVolumeEvent : FWEvent<ChangeVolumeEvent>
{
    public MixerType MixerType { get; private set; }
    public float Volume { get; private set; }

    public ChangeVolumeEvent(MixerType mixerType, float volume)
    {
        MixerType = mixerType;
        Volume = volume;
    }
}