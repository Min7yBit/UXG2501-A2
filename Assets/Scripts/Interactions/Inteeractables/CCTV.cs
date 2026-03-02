using UnityEngine;

public class CCTV : MonoBehaviour, IInteractable
{
    public string Name => name;

    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;
    public bool interactable;
    public bool interacted = false;
    private void Awake()
    {
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (!interactable)
            return;
        if (!interacted)
        {
            interacted = true;
            UIManager uiManager = FindFirstObjectByType<UIManager>();
            uiManager.UpdateHintsCount();
        }

        this.gameObject.GetComponent<Dialogue>().InitialiseDialogue();
    }
}
