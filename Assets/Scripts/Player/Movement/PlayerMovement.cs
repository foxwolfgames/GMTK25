using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform spriteTransform;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 14f;
    [SerializeField] private float acceleration = 120f;
    [SerializeField] private float deceleration = 60f;
    [SerializeField] private float airAcceleration = 120f;
    [SerializeField] private float airDeceleration = 30f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 36f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpEndEarlyGravityMultiplier = 3f;

    [Header("Gravity")]
    [SerializeField] private float maxFallSpeed = 40f;
    [SerializeField] private float fallAcceleration = 110f;
    [SerializeField] private float groundingForce = -1.5f;

    [Header("Debug")]
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private bool isGrounded;

    private Rigidbody2D rigidBody;
    private CapsuleCollider2D capsuleCollider;
    private bool cachedQueryStartInColliders;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 movementInput;
    private bool jumpHeld;
    private bool jumpPressed;
    private float lastJumpPressedTime;
    private float lastGroundedTime;
    private float timeLeftGrounded;
    private bool jumpEndedEarly;

    public Vector2 Direction;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        playerInput = GetComponent<PlayerInput>();
        Physics2D.queriesStartInColliders = false;
    }

    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        moveAction.performed += context => movementInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => movementInput = Vector2.zero;

        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += context =>
        {
            jumpPressed = true;
            jumpHeld = true;
            lastJumpPressedTime = Time.time;
        };
        jumpAction.canceled += context => jumpHeld = false;
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleHorizontalMovement();
        HandleJump();
        HandleGravity();

        rigidBody.linearVelocity = currentVelocity;
    }

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        Vector2 capsuleCenter = capsuleCollider.bounds.center;
        Vector2 capsuleSize = capsuleCollider.size;
        CapsuleDirection2D capsuleDir = capsuleCollider.direction;
        float checkDistance = 0.05f;
        LayerMask platformMask = groundLayer;

        // Ground and ceiling checks
        bool groundHit = Physics2D.CapsuleCast(capsuleCenter, capsuleSize, capsuleDir, 0, Vector2.down, checkDistance, platformMask);
        bool ceilingHit = Physics2D.CapsuleCast(capsuleCenter, capsuleSize, capsuleDir, 0, Vector2.up, checkDistance, platformMask);

        // Cancel vertical velocity if ceiling hit
        if (ceilingHit && currentVelocity.y > 0)
        {
            currentVelocity.y = 0;
        }

        // Handle ground state
        bool wasGrounded = isGrounded;

        if (!wasGrounded && groundHit)
        {
            isGrounded = true;
            jumpEndedEarly = false;
            lastGroundedTime = Time.time;
        }
        else if (wasGrounded && !groundHit)
        {
            isGrounded = false;
            timeLeftGrounded = Time.time;
        }

        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;
    }

    private void HandleHorizontalMovement()
    {
        float targetSpeed = movementInput.x * moveSpeed;

        float accel = isGrounded
            ? (Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration)
            : (Mathf.Abs(targetSpeed) > 0.01f ? airAcceleration : airDeceleration);

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);

        // Flip sprite
        if (movementInput.x > 0.01f)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
            Direction = Vector2.right;
        }
        else if (movementInput.x < -0.01f)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
            Direction = Vector2.left;
        }
    }

    private void HandleJump()
    {
        bool canCoyote = !isGrounded && Time.time - timeLeftGrounded <= coyoteTime;
        bool canBuffered = Time.time - lastJumpPressedTime <= jumpBufferTime;

        if ((isGrounded || canCoyote) && canBuffered)
        {
            currentVelocity.y = jumpForce;
            jumpEndedEarly = false;
            lastJumpPressedTime = float.MinValue;
            lastGroundedTime = float.MinValue;
        }

        if (!jumpHeld && currentVelocity.y > 0 && !jumpEndedEarly)
        {
            currentVelocity.y *= 0.5f;
            jumpEndedEarly = true;
        }
    }

    private void HandleGravity()
    {
        if (isGrounded && currentVelocity.y <= 0f)
        {
            currentVelocity.y = groundingForce;
        }
        else
        {
            float gravity = fallAcceleration;

            if (jumpEndedEarly && currentVelocity.y > 0)
                gravity *= jumpEndEarlyGravityMultiplier;

            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -maxFallSpeed, gravity * Time.fixedDeltaTime);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (capsuleCollider == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(capsuleCollider.bounds.center + Vector3.down * 0.05f, capsuleCollider.bounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(capsuleCollider.bounds.center + Vector3.up * 0.05f, capsuleCollider.bounds.size);
    }

}
