using System.Collections;
using TMPro;
using UnityEngine;

public class Screw : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Required Item")]
    public string itemName;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;      // assign in inspector
    [SerializeField] private AudioClip screwRemoveSFX;     // assign sound clip
    [Range(0f, 2f)] public float screwVolume = 1f;

    [SerializeField] private BedLeg bedLeg;
    private bool interactable = true;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered Bed Clickable Area " + name);
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited Bed Clickable Area " + name);
    }

    private void OnMouseOver()
    {
        if (!interactable) return;

        if (Input.GetMouseButton(0))
        {
            if (inventory.ContainsItem(itemName))
            {
                // Remove the required item
                inventory.RemoveItem(inventory.GetItem(itemName));
                Debug.Log("Screw Removed");

                //  PLAY SCREW REMOVAL SOUND
                PlayScrewRemoveSFX();

                interactable = false;
                bedLeg.canRemove = true;

                // Disable screw object
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Cannot Remove screw, required item not present");
                ShowMessage("Cannot remove Screw, required item not present");
            }
        }
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        // Interaction done via mouse clicks only
    }

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
    // AUDIO FUNCTION
    // ---------------------------
    private void PlayScrewRemoveSFX()
    {
        if (audioSource != null && screwRemoveSFX != null)
        {
            audioSource.PlayOneShot(screwRemoveSFX, screwVolume);
        }
        else
        {
            Debug.LogWarning("[Screw] Missing AudioSource or screwRemoveSFX!");
        }
    }
}
