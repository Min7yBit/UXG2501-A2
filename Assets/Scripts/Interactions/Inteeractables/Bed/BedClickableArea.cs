using System.Collections;
using TMPro;
using UnityEngine;

public class BedClickableArea : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool ShowPrompt { get; set; } = false;
    public bool InInteract { get; set; } = false;

    public CameraControl cameraControl;
    public Camera cam;

    private bool interactable = false;
    private bool mouseOver = false;
    private PlayerMovement playerMovement;
    [SerializeField] private Collider col;
    [SerializeField]private Collider bedCol;
    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered Bed Clickable Area");
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited Bed Clickable Area");
        mouseOver = false;
    }
    public void OnInteract(in PlayerMovement playerMovement)
    {
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && CanInteract)
        {
            InInteract = true;
            Debug.Log("Interacted with " + name);
            cameraControl.SwitchToFixedCamera(cam);
            col.enabled = false; //disables interaction so can interact with child components
            bedCol.enabled = false; //disables bed collider
            ShowMessage("What's here?");
        }
    }

    private void Update()
    {
        if (InInteract)
        {

            if (Input.GetKeyDown(KeyCode.B))
            {
                InInteract = false;
                col.enabled = true; //enables interaction again
                bedCol.enabled = true; //enables bed collider again
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                interactable = true;
                cameraControl.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
            }
        }
    }

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI interactionPrompt;

    public void ShowMessage(string msg, float duration = 2f)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        StartCoroutine(FadeMessage(duration));
    }

    private IEnumerator FadeMessage(float duration)
    {
        // Reset alpha to fully visible
        Color c = messageText.color;
        c.a = 1;
        messageText.color = c;

        // Fade out over time
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, elapsed / duration);
            messageText.color = c;
            yield return null;
        }

        // Hide once fully faded
        messageText.gameObject.SetActive(false);
    }
}
