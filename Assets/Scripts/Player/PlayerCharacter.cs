using Unity.Cinemachine;
using UnityEngine;
public class PlayerCharacter : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public CinemachineCamera virtualCamera;
    public Transform SpellSpawnPoint;
    public bool isFacingRight = true;
}
