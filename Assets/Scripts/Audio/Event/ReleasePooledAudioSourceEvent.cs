using FWGameLib.Common.EventSystem;

namespace Chronomance.Audio
{
    public class ReleasePooledAudioSourceEvent : FWEvent<ReleasePooledAudioSourceEvent>
    {
        public readonly PooledAudioSource Resource;

        public ReleasePooledAudioSourceEvent(PooledAudioSource resource)
        {
            Resource = resource;
        }
    }
}