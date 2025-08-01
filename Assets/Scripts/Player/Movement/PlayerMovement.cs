using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;

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
    private int timesJumped = 0;
    [SerializeField] bool CanDoubleJump = true;

    [Header("Gravity")]
    [SerializeField] private float maxFallSpeed = 40f;
    [SerializeField] private float fallAcceleration = 110f;
    [SerializeField] private float groundingForce = -1.5f;

    [Header("Debug")]
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private bool isGrounded;
    private bool wasGrounded = false;

    [Header("Events")]
    public UnityEvent Landed;


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

    #region Unity Methods

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
        moveAction.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => movementInput = Vector2.zero;

        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += _ =>
        {
            jumpPressed = true;
            jumpHeld = true;
            lastJumpPressedTime = Time.time;
        };
        jumpAction.canceled += _ => jumpHeld = false;
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleHorizontalMovement();
        HandleJump();
        HandleGravity();

        rigidBody.linearVelocity = currentVelocity;
    }

    #endregion

    #region Movement Methods

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        Vector2 capsuleCenter = capsuleCollider.bounds.center;
        Vector2 capsuleSize = capsuleCollider.size;
        CapsuleDirection2D capsuleDir = capsuleCollider.direction;
        float checkDistance = 0.05f; // Adjust if needed
        LayerMask platformMask = groundLayer; // Or ~playerLayer if you have it

        // Ground and Ceiling Checks
        bool groundHit = Physics2D.CapsuleCast(capsuleCenter, capsuleSize, capsuleDir, 0, Vector2.down, checkDistance, platformMask);
        bool ceilingHit = Physics2D.CapsuleCast(capsuleCenter, capsuleSize, capsuleDir, 0, Vector2.up, checkDistance, platformMask);

        // Cancel vertical velocity if ceiling hit
        if (ceilingHit && currentVelocity.y > 0)
        {
            currentVelocity.y = 0;
        }

        // Handle ground state transitions
        //bool wasGrounded = isGrounded;

        if (!wasGrounded && groundHit)
        {
            isGrounded = true;
            jumpEndedEarly = false;
            lastGroundedTime = Time.time;

            timesJumped = 0;
            Landed?.Invoke();
        }
        else //if (wasGrounded && !groundHit)
        {
            isGrounded = false;
            timeLeftGrounded = Time.time;
            wasGrounded = isGrounded;
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
    }

    private void HandleJump()
    {
        bool canCoyote = !isGrounded && Time.time - timeLeftGrounded <= coyoteTime;
        bool canBuffered = Time.time - lastJumpPressedTime <= jumpBufferTime;

        if (!CanDoubleJump || timesJumped >= 2)
        {
            return;
        }

        if ((isGrounded || canCoyote) && canBuffered)
        {
            currentVelocity.y = jumpForce;
            jumpEndedEarly = false;
            lastJumpPressedTime = float.MinValue;
            lastGroundedTime = float.MinValue;
            timesJumped++;
        }

        if (!jumpHeld && currentVelocity.y > 0 && !jumpEndedEarly)
        {
            currentVelocity.y *= 0.5f;
            jumpEndedEarly = true;
        }
    }

    private void HandleGravity()
    {
        if (isGrounded && currentVelocity.y <= 0.01f)
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

    #endregion
}
