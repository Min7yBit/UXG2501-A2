using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 12f;
    public float deceleration = 10f;
    public float airControl = 0.4f;

    [Header("Camera Look")]
    public float lookSensitivity = 2f;
    public Transform cameraHolder;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    [Header("SFX Settings")]
    public AudioSource audioSource;

    [Header("Footsteps")]
    public AudioClip[] footstepClips;
    public float footstepInterval = 0.45f;
    public float footstepVolume = 0.8f;

    [Header("Jump SFX")]
    public AudioClip jumpClip;
    [Range(0f, 2f)] public float jumpVolume = 1f;

    [Header("Landing SFX")]
    public AudioClip landClip;
    [Range(0f, 2f)] public float landVolume = 1f;

    private Rigidbody rb;
    private float rotationX;

    private bool canMove = true;
    public bool CanMove { get => canMove; set => canMove = value; }

    // INPUT
    private float inputX;
    private float inputZ;
    private bool jumpPressed;

    // FOOTSTEPS / LANDING
    private float footstepTimer;
    private bool wasGrounded = true;

    // NEW: used to block footsteps during jump until we land
    private bool suppressFootstepsUntilGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!CanMove) return;

        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        RotatePlayer();
    }

    void FixedUpdate()
    {
        if (!CanMove) return;

        MovePlayer();
        HandleJump();
        HandleFootstepsAndLanding();
    }

    // -------------------------------------------------------
    // MOVEMENT
    // -------------------------------------------------------
    void MovePlayer()
    {
        Vector3 inputDir = (transform.right * inputX + transform.forward * inputZ).normalized;

        Vector3 currentVel = rb.linearVelocity;
        Vector3 horizontalVel = new Vector3(currentVel.x, 0, currentVel.z);

        float control = IsGrounded() ? 1f : airControl;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            Vector3 target = inputDir * moveSpeed;
            Vector3 newVel = Vector3.MoveTowards(horizontalVel, target,
                acceleration * control * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector3(newVel.x, currentVel.y, newVel.z);
        }
        else
        {
            Vector3 newVel = Vector3.MoveTowards(horizontalVel, Vector3.zero,
                deceleration * control * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector3(newVel.x, currentVel.y, newVel.z);
        }
    }

    // -------------------------------------------------------
    // JUMP
    // -------------------------------------------------------
    void HandleJump()
    {
        if (jumpPressed && IsGrounded())
        {
            Vector3 vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            // we just jumped – block footsteps until we land
            suppressFootstepsUntilGrounded = true;

            PlayJumpSFX();
        }

        jumpPressed = false;
    }

    // -------------------------------------------------------
    // FOOTSTEPS + LANDING
    // -------------------------------------------------------
    void HandleFootstepsAndLanding()
    {
        bool grounded = IsGrounded();

        // LANDING
        if (!wasGrounded && grounded)
        {
            PlayLandSFX();
            // allow footsteps again after landing
            suppressFootstepsUntilGrounded = false;
        }

        wasGrounded = grounded;

        // FOOTSTEPS (only when grounded, moving, and not in "jump phase")
        if (grounded && !suppressFootstepsUntilGrounded && IsMoving())
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstepSFX();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    // -------------------------------------------------------
    // HELPERS
    // -------------------------------------------------------
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down,
            groundCheckDistance, groundLayer);
    }

    public bool IsMoving()
    {
        return Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputZ) > 0.01f;
    }

    // -------------------------------------------------------
    // SFX
    // -------------------------------------------------------
    void PlayFootstepSFX()
    {
        if (footstepClips.Length == 0 || audioSource == null) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip, footstepVolume);
    }

    void PlayJumpSFX()
    {
        if (jumpClip != null && audioSource != null)
            audioSource.PlayOneShot(jumpClip, jumpVolume);
    }

    void PlayLandSFX()
    {
        if (landClip != null && audioSource != null)
            audioSource.PlayOneShot(landClip, landVolume);
    }

    // -------------------------------------------------------
    // CAMERA
    // -------------------------------------------------------
    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cameraHolder.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    // -------------------------------------------------------
    // DEBUG
    // -------------------------------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawWireSphere(
            transform.position + Vector3.down * groundCheckDistance,
            0.05f);
    }
}
