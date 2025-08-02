using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Tooltip("Maximum distance enemy follows player")]
    [SerializeField] float followRange;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpCoolDown;

    float jumpCoolDownTime;

    public bool isGrounded;
    bool chase;

    void Update()
    {
        jumpCoolDownTime -= Time.deltaTime;
        chase = (transform.position - playerTransform.position).magnitude <= followRange;
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, .1f, groundLayer);
    }

    void FixedUpdate()
    {
        if (chase)
        {
            rb.AddRelativeForceX((transform.position.x < playerTransform.position.x) ?
                moveSpeed : -moveSpeed, ForceMode2D.Force);
        }
        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);

    }
    /// <summary>
    /// Apply vertical force
    /// </summary>
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpCoolDownTime = jumpCoolDown;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Jump Node") && isGrounded && jumpCoolDownTime <= 0)
        {
            Jump();
        }
    }
}
