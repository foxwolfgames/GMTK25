using FWGameLib.Common.EventSystem;

// Event fired when the player's mana value is updated
public class ManaValueUpdateEvent : FWEvent<ManaValueUpdateEvent>
{
    public int NewMana;

    public ManaValueUpdateEvent(int newMana)
    {
        NewMana = newMana;
    }
}