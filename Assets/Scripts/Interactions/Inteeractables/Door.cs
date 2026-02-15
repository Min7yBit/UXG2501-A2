using System.Collections;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    public CameraControl cameraControl;
    public Camera cam;
    public GameObject lockReflection;

    [SerializeField] private TextMeshProUGUI interactPrompt;
    [SerializeField] private GameObject interactPromptGO;

    public string itemName;

    private PlayerMovement playerMovement;
    private bool interactable = true;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Collider mirrorShardCol;

    [Header("Door Open SFX")]
    public AudioSource audioSource;        // assign in inspector
    public AudioClip doorOpenSFX;          // assign your door sound
    [Range(0f, 2f)] public float doorOpenVolume = 1f;

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (inventory.ContainsItem(itemName))
        {
            // ---------------------------
            // PLAY DOOR OPEN SFX HERE
            // ---------------------------
            PlayDoorOpenSFX();

            InInteract = true;
            lockReflection.SetActive(true);
            mirrorShardCol.enabled = true;
            interactable = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.playerMovement = playerMovement;
            this.playerMovement.CanMove = false;

            cameraControl.SwitchToFixedCamera(cam);
        }
        else
        {
            Debug.Log("I can't see the lock...");
            ShowMessage("I can't see the lock...", 2);
        }
    }

    private void Update()
    {
        if (InInteract)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                InInteract = false;
                mirrorShardCol.enabled = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                interactable = true;
                playerMovement.CanMove = true;

                cameraControl.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
            }
        }
    }

    // -------------------------------------------------------
    // SFX METHOD
    // -------------------------------------------------------
    private void PlayDoorOpenSFX()
    {
        if (audioSource != null && doorOpenSFX != null)
        {
            audioSource.PlayOneShot(doorOpenSFX, doorOpenVolume);
        }
    }

    // -------------------------------------------------------
    // UI MESSAGE
    // -------------------------------------------------------
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
