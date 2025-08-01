using FWGameLib.Common.EventSystem;

namespace Chronomance.Audio
{
    public class ChangeVolumeEvent : FWEvent<ChangeVolumeEvent>
    {
        public enum MixerType
        {
            Master,
            Music,
            SoundEffects
        }

        public readonly MixerType Type;
        public readonly float Volume;

        public ChangeVolumeEvent(MixerType type, float volume)
        {
            Type = type;
            Volume = volume;
        }
    }
}