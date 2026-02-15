using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private Rigidbody rb;

    [Header("References")]
    public PlayerMovement movementScript;  // Drag PlayerMovement here

    private bool wasGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (movementScript == null)
            movementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        UpdateMovementAnimation();
        UpdateJumpAndLandingAnimation();
    }

    void UpdateMovementAnimation()
    {
        // Convert world velocity to local space relative to character
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

        animator.SetFloat("MoveX", localVel.x);  // Left/Right
        animator.SetFloat("MoveZ", localVel.z);  // Forward/Back
        if (localVel.x > 0 || localVel.z > 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void UpdateJumpAndLandingAnimation()
    {
        bool isGrounded = movementScript.IsGrounded();

        // Jump / fall animation data
        animator.SetBool("IsGrounded", isGrounded);
        //animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

        // Detect landing
        if (!wasGrounded && isGrounded)
        {
            // Just landed this frame
            animator.SetTrigger("Landed");
        }

        wasGrounded = isGrounded;
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
