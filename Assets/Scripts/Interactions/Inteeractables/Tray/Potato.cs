using UnityEngine;
using System.Collections;

public class Potato : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Zoom Settings")]
    public float zoomDuration = 1.0f;
    public Vector3 zoomedPosition;
    public Vector3 rotatePosition;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Renderer Rrenderer;
    [SerializeField] private bool mouseOver = false;
    private bool interactable = true;
    [SerializeField] private bool zoomedIn = false;
    private bool allowRotation = false;

    [Header("Rotation Settings")]
    public int intervals = 4;
    public float rotationDuration = 0.25f;
    [SerializeField] private int currentStepIndex = 0;
    private float stepAngle;
    private Coroutine rotateCoroutine;
    private bool rotating = false;

    [Header("SFX")]
    public AudioSource audioSource;       // assign in inspector
    public AudioClip pickUpSFX;           // when zooming in first time
    public AudioClip clickRotateSFX;      // each click/rotation
    [Range(0f, 2f)] public float pickUpVolume = 1f;
    [Range(0f, 2f)] public float clickVolume = 1f;

    [SerializeField] private UIManager uIManager;
    private bool revealedHint = false;

    private void Awake()
    {
        Rrenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        if (intervals <= 0)
        {
            Debug.LogError("Intervals must be a positive integer!");
            intervals = 1;
        }

        stepAngle = 360f / intervals;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void Update()
    {
        if (zoomedIn && !mouseOver && interactable && Input.GetMouseButtonDown(0))
        {
            interactable = false;
            ZoomOut();
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered Potato");
        Rrenderer.material.color = Color.yellow;
        mouseOver = true;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOver && zoomedIn)
            {
                // CLICK WHILE ZOOMED IN  ROTATE + CLICK SFX
                PlayClickSFX();
                RotateToNextInterval();
            }
            else if (!zoomedIn && mouseOver && interactable)
            {
                // FIRST PICK UP / ZOOM IN  PICKUP SFX
                interactable = false;
                PlayPickUpSFX();
                ZoomIn();
            }
        }
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited Potato");
        Rrenderer.material.color = Color.white;
        mouseOver = false;
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        // not used
    }

    private void ZoomIn()
    {
        StartCoroutine(ZoomCoroutine(transform.localPosition, zoomedPosition, zoomDuration));
        StartCoroutine(RotateCoroutine(transform.localRotation, Quaternion.Euler(rotatePosition), zoomDuration));
    }

    private void ZoomOut()
    {
        StartCoroutine(ZoomCoroutine(transform.localPosition, initialPosition, zoomDuration));
        StartCoroutine(RotateCoroutine(transform.localRotation, initialRotation, zoomDuration));
    }

    private IEnumerator ZoomCoroutine(Vector3 startPos, Vector3 endPos, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPos;

        // toggles
        zoomedIn = !zoomedIn;
        allowRotation = !allowRotation;
        interactable = true;
    }

    private IEnumerator RotateCoroutine(Quaternion startPos, Quaternion endPos, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.localRotation = Quaternion.Lerp(startPos, endPos, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endPos;
    }

    public void ResetPotato()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
        zoomedIn = false;
        allowRotation = false;
        interactable = false;
    }

    private void RotateToNextInterval()
    {
        if (rotating || !allowRotation)
            return;

        Quaternion startRotation = transform.localRotation;
        Quaternion stepRotation = Quaternion.AngleAxis(stepAngle, Vector3.up);
        Quaternion targetRotation = startRotation * stepRotation;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateSmoothly(startRotation, targetRotation));

        currentStepIndex = (currentStepIndex + 1) % intervals;

        if (currentStepIndex == 3 && !revealedHint)
        {
            revealedHint = true;
            uIManager.UpdateHintsCount();
        }
    }

    IEnumerator RotateSmoothly(Quaternion startRot, Quaternion endRot)
    {
        rotating = true;
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            float t = timeElapsed / rotationDuration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRot;
        rotateCoroutine = null;
        rotating = false;
    }

    // -------------------------
    // SFX HELPERS
    // -------------------------
    private void PlayPickUpSFX()
    {
        if (audioSource != null && pickUpSFX != null)
            audioSource.PlayOneShot(pickUpSFX, pickUpVolume);
    }

    private void PlayClickSFX()
    {
        if (audioSource != null && clickRotateSFX != null)
            audioSource.PlayOneShot(clickRotateSFX, clickVolume);
    }
}
