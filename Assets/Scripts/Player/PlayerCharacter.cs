using Unity.Cinemachine;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public CinemachineCamera virtualCamera;
    public Transform SpellSpawnPoint;
    public bool IsFacingRight = true;

    private void OnEnable()
    {
        PlayerTeleportEvent.Handler += OnPlayerTeleport;
    }

    private void OnDisable()
    {
        PlayerTeleportEvent.Handler -= OnPlayerTeleport;
    }

    private void OnPlayerTeleport(PlayerTeleportEvent e)
    {
        transform.position = e.Position;
    }
}
