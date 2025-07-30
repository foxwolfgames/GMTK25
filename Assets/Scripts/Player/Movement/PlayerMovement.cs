using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementInput;
    private Rigidbody2D rigidBody;
    private PlayerInput playerInput;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    [Tooltip("Due to clamp value, acc. must be < 1")]
    private float acceleration = 10f;

    [SerializeField]
    [Header("Debug Values")]
    private Vector2 currentVelocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
            Debug.LogWarning("Rigidbody component not found.");
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        InputAction playerActions = playerInput.actions["Move"];
        playerActions.performed += context => movementInput = context.ReadValue<Vector2>();
        playerActions.canceled += context => movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = movementInput.normalized * moveSpeed;
        
        // Modify Velocity:
        LerpVelocity(targetVelocity);
        // other modifiers like external force?

        // Clamp velocity
        if (currentVelocity.magnitude < 0.1f)
        {
            currentVelocity = Vector2.zero;
        }
        rigidBody.linearVelocity = currentVelocity;
    }

    private void LerpVelocity(Vector2 targetVelocity)
    {
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
}
