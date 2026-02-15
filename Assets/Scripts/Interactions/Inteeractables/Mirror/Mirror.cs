using System.Collections;
using TMPro;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    public string Name => name;

    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Interaction / Inventory")]
    public string itemName;
    [SerializeField] private Item shardItem;
    [SerializeField] private GameObject mirrorMess;
    [SerializeField] private Inventory inventory;
    [SerializeField] private bool interactable;

    [Header("Shatter SFX")]
    [SerializeField] private AudioSource audioSource;   // assign in Inspector
    [SerializeField] private AudioClip shatterSFX;      // glass breaking sound
    [Range(0f, 2f)] public float shatterVolume = 1f;

    private bool mouseOver = false;

    public TextMeshProUGUI messageText;

    private void Awake()
    {
        // optional: renderer stuff if you re-enable it later
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered " + name);
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited " + name);
        mouseOver = false;
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (!mouseOver) return;

        if (inventory.ContainsItem(itemName) && shardItem != null) // has bed leg, can break
        {
            //  PLAY SHATTER SFX HERE
            PlayShatterSFX();

            inventory.RemoveItem(inventory.GetItem(itemName));              // remove bed leg
            playerMovement.GetComponent<Inventory>().AddItem(shardItem);    // add shard
            interactable = false;

            mirrorMess.SetActive(true);     // show broken mirror
            gameObject.SetActive(false);    // hide this mirror

            Debug.Log("Mirror Broken");
        }
        else
        {
            Debug.Log("Hmm maybe I can break this with something.");
            ShowMessage("Hmm... maybe I can break this with something");
        }
    }

    private void PlayShatterSFX()
    {
        if (audioSource != null && shatterSFX != null)
        {
            audioSource.PlayOneShot(shatterSFX, shatterVolume);
        }
        else
        {
            Debug.LogWarning("[Mirror] Missing AudioSource or shatterSFX clip.");
        }
    }

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
}
