using FWGameLib.Common.EventSystem;

public class RoundStartEvent : FWEvent<RoundStartEvent>
{
    public int IncomingRoundNumber;

    public RoundStartEvent(int incomingRoundNumber)
    {
        IncomingRoundNumber = incomingRoundNumber;
    }
}