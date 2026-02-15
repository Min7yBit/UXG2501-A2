using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    public string Name => name;

    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;
    public bool interactable;
    private Renderer Rrenderer;
    private bool mouseOver = false;
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
    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (!interactable || !mouseOver)
            return;

        Debug.Log("Interacted with " + name);

        Item item = GetComponent<Item>();
        if (item != null)
        {
            playerMovement.GetComponent<Inventory>().AddItem(item); //testing adding item to inventory on interact, some items may not have Item component
            gameObject.SetActive(false); //disable the object after picking it up
        }
    }
}
