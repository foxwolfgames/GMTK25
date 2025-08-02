using UnityEngine;

namespace Chronomance.Terrain
{
    public class Spawnpoint : MonoBehaviour
    {
        private void OnEnable()
        {
            RoundStartEvent.Handler += OnRoundStart;
        }

        private void OnDisable()
        {
            RoundStartEvent.Handler -= OnRoundStart;
        }

        private void OnRoundStart(RoundStartEvent _)
        {
            new PlayerTeleportEvent(transform.position).Invoke();
        }
    }
}