using UnityEngine;
using System.Collections;

public class PosterReveal : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Hint Behind Poster")]
    [SerializeField] private GameObject hintObject;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.4f;
    [SerializeField] private Vector3 flipAxis = new Vector3(0f, 1f, 0f);
    [SerializeField] private UIManager uIManager;

    [Header("Tear SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tearSFX;
    [Range(0f, 2f)] public float tearVolume = 1f;

    private bool interactable = true;
    private bool hasFlipped = false;
    private bool isFlipping = false;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    public Transform GetTransform() => transform;

    private void Awake()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.AngleAxis(180f, flipAxis.normalized);

        if (hintObject != null)
            hintObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered Poster area");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited Poster area");
    }

    private void OnMouseOver()
    {
        if (!interactable || isFlipping || hasFlipped)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            //  Play SFX **RIGHT WHEN CLICKED**
            PlayTearSFX();

            // Start animation
            StartCoroutine(FlipPoster());
        }
    }

    private IEnumerator FlipPoster()
    {
        isFlipping = true;
        interactable = false;

        float t = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = targetRotation;

        while (t < 1f)
        {
            t += Time.deltaTime / flipDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;
        hasFlipped = true;
        isFlipping = false;

        if (hintObject != null)
            hintObject.SetActive(true);

        Debug.Log("Poster flipped, hint revealed.");
        uIManager.UpdateHintsCount();
    }

    private void PlayTearSFX()
    {
        if (audioSource != null && tearSFX != null)
            audioSource.PlayOneShot(tearSFX, tearVolume);
        else
            Debug.LogWarning("[PosterReveal] Missing AudioSource or tearSFX!");
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        // Not used for poster
    }
}
