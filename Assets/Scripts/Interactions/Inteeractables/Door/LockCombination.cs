using UnityEngine;
using System.Collections; // Required for Coroutines

public class LockCombination : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;
    private bool interactable = false;

    [Header("Combination Settings")]
    public int winIndex = 0; // set this individually in inspector to define the correct combination
    public int intervals = 4;
    public float rotationDuration = 0.25f;
    [SerializeField] public int currentStepIndex = 0; // Tracks which step we are currently at
    private float stepAngle;         // The angle of a single step (360 / intervals)
    private Coroutine rotateCoroutine; // Reference to the running coroutine
    private bool isRotating = false;

    [Header("Dial Spin SFX")]
    public AudioSource audioSource;       // assign in Inspector
    public AudioClip spinSFX;             // assign spin/click sound
    [Range(0f, 2f)] public float spinVolume = 1f;

    void Start()
    {
        // Safety check to prevent division by zero and nonsensical rotations
        if (intervals <= 0)
        {
            Debug.LogError("Intervals must be a positive integer!");
            intervals = 1;
        }

        // Calculate the fixed angle for each interval
        stepAngle = 360f / intervals;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        //must be in contact with it to work!!!
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && CanInteract)
        {
            RotateToNextInterval();
        }
    }

    private void RotateToNextInterval()
    {
        if (isRotating) return;

        Quaternion startRotation = transform.localRotation;

        // Use quaternion multiplication instead of Euler reads
        Quaternion stepRotation = Quaternion.Euler(0f, stepAngle, 0f); // rotate on Y

        Quaternion targetRotation = startRotation * stepRotation;

        //  PLAY SPIN SFX WHEN DIAL TURNS
        PlaySpinSFX();

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateSmoothly(startRotation, targetRotation));

        currentStepIndex = (currentStepIndex + 1) % intervals;
    }

    IEnumerator RotateSmoothly(Quaternion startRot, Quaternion endRot)
    {
        isRotating = true;
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            float t = timeElapsed / rotationDuration;

            // Use Quaternion.Slerp for smooth rotation
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the rotation lands exactly on the target angle
        transform.rotation = endRot;
        rotateCoroutine = null;

        GetComponentInParent<WinCondition>().CheckWinCondition();
        isRotating = false;
    }

    public void ResetCombination()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }

        Quaternion resetRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            90f,
            transform.localEulerAngles.z
        );

        rotateCoroutine = StartCoroutine(RotateSmoothly(transform.rotation, resetRotation));
        currentStepIndex = 0;
    }

    public bool IsCorrectCombination()
    {
        return currentStepIndex == winIndex;
    }

    // -----------------------------
    // SFX HELPER
    // -----------------------------
    private void PlaySpinSFX()
    {
        if (audioSource != null && spinSFX != null)
        {
            audioSource.PlayOneShot(spinSFX, spinVolume);
        }
        else
        {
            // Optional: comment this out if it gets spammy
            Debug.LogWarning("[LockCombination] Missing AudioSource or spinSFX!");
        }
    }
}
