using System.Collections;
using TMPro;
using UnityEngine;

public class CrackWall : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Required Item")]
    public string itemName = "Spoon";

    [Header("Scrape Settings")]
    public int scrapesNeeded = 3;
    private int currentScrapes = 0;

    [Header("Cell / Wall Objects")]
    [SerializeField] private GameObject intactCell;
    [SerializeField] private GameObject holedCell;
    [SerializeField] private GameObject hintObject;
    [SerializeField] private GameObject crack;

    [Header("Player Inventory")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private UIManager uIManager;

    [Header("Scrape SFX")]
    public AudioSource audioSource;
    public AudioClip scrapeSFX;       // sound each time spoon scrapes
    public AudioClip breakSFX;        // optional break sound
    [Range(0f, 2f)] public float scrapeVolume = 1f;
    [Range(0f, 2f)] public float breakVolume = 1f;

    private bool interactable = true;
    private bool mouseOver = false;

    public Transform GetTransform() => transform;

    private void Awake()
    {
        if (intactCell != null) intactCell.SetActive(true);
        if (holedCell != null) holedCell.SetActive(false);
        if (hintObject != null) hintObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
        Debug.Log("Mouse Entered CrackWall area");
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        Debug.Log("Mouse Exited CrackWall area");
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void RevealHint()
    {
        Debug.Log("Wall broken, revealing hole and hint.");

        interactable = false;

        if (intactCell != null)
            intactCell.SetActive(false);

        if (holedCell != null)
            holedCell.SetActive(true);

        if (hintObject != null)
            hintObject.SetActive(true);

        if (crack != null)
            crack.SetActive(false);

        // PLAY BREAK SOUND
        PlayBreakSFX();

        gameObject.SetActive(false);
        uIManager.UpdateHintsCount();
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (!interactable)
            return;

        if (Input.GetMouseButtonDown(0) && mouseOver)
        {
            if (inventory == null)
            {
                Debug.LogWarning("CrackWall: Inventory reference not assigned in Inspector.");
                return;
            }

            if (inventory.ContainsItem(itemName))
            {
                currentScrapes++;
                Debug.Log($"Scraping wall with {itemName}... ({currentScrapes}/{scrapesNeeded})");

                //  PLAY SCRAPE SFX
                PlayScrapeSFX();

                if (currentScrapes >= scrapesNeeded)
                {
                    RevealHint();
                }
            }
            else
            {
                Debug.Log($"Cannot scrape wall, required item not selected: {itemName}");
                ShowMessage($"Cannot scrape wall, required item not selected: {itemName}");
            }
        }
    }

    // -------------------------
    // SCRAPE SFX PLAYBACK
    // -------------------------
    private void PlayScrapeSFX()
    {
        if (audioSource != null && scrapeSFX != null)
            audioSource.PlayOneShot(scrapeSFX, scrapeVolume);
        else
            Debug.LogWarning("[CrackWall] Missing AudioSource or scrapeSFX!");
    }

    private void PlayBreakSFX()
    {
        if (audioSource != null && breakSFX != null)
            audioSource.PlayOneShot(breakSFX, breakVolume);
    }

    // -------------------------
    // MESSAGE UI
    // -------------------------
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
}
