using FWGameLib.Common.EventSystem;
using UnityEngine;

public class PlayerTeleportEvent : FWEvent<PlayerTeleportEvent>
{
    public readonly Vector3 Position;

    public PlayerTeleportEvent(Vector3 position)
    {
        Position = position;
    }
}