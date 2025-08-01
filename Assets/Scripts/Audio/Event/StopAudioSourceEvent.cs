using System;
using FWGameLib.Common.EventSystem;

namespace Chronomance.Audio
{
    public class StopAudioSourceEvent : FWEvent<StopAudioSourceEvent>
    {
        public readonly Guid AudioSourceId;

        public StopAudioSourceEvent(Guid audioSourceId)
        {
            AudioSourceId = audioSourceId;
        }
    }
}