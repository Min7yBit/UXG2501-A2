using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BedLeg : MonoBehaviour, IInteractable
{
    public string Name => name;

    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;
    public bool canRemove = false;

    private bool interactable;
    private Renderer Rrenderer;
    private bool mouseOver = false;

    [SerializeField] private PlayerMovement playerMovement;

    [Header("Pickup SFX")]
    [SerializeField] private AudioSource audioSource;       // assign in inspector
    [SerializeField] private AudioClip pickupSFX;           // assign pickup sound
    [Range(0f, 2f)] public float pickupVolume = 1f;

    private void Awake()
    {
        Rrenderer = GetComponent<Renderer>();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnMouseEnter()
    {
        if (!interactable)
            return;

        Debug.Log("Mouse Entered " + name);
        Rrenderer.material.color = Color.yellow;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (!interactable)
            return;

        Debug.Log("Mouse Exited " + name);
        Rrenderer.material.color = Color.white;
        mouseOver = false;
    }

    private void OnMouseOver()
    {
        if (!interactable)
            return;

        if (Input.GetMouseButton(0))
        {
            if (!canRemove)
            {
                ShowMessage("I can't remove it with the screw on.");
                Debug.Log("I can't remove it with the screw on.");
                return;
            }

            Debug.Log("Interacted with " + name);
            Item item = GetComponent<Item>();
            if (item != null)
            {
                // Add to inventory
                playerMovement.GetComponent<Inventory>().AddItem(item);

                //  PLAY PICKUP SFX
                PlayPickupSFX();

                // Disable object after pickup
                gameObject.SetActive(false);
            }
        }
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        // Unused (handled via mouse clicks)
    }

    [Header("UI Message")]
    public TextMeshProUGUI messageText;

    public void ShowMessage(string msg, float duration = 2f)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        StartCoroutine(FadeMessage(duration));
    }

    private IEnumerator FadeMessage(float duration)
    {
        Color c = messageText.color;
        c.a = 1;
        messageText.color = c;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, elapsed / duration);
            messageText.color = c;
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }

    // ---------------------------
    // AUDIO
    // ---------------------------
    private void PlayPickupSFX()
    {
        if (audioSource != null && pickupSFX != null)
        {
            audioSource.PlayOneShot(pickupSFX, pickupVolume);
        }
        else
        {
            Debug.LogWarning("[BedLeg] Missing AudioSource or pickupSFX!");
        }
    }
}
