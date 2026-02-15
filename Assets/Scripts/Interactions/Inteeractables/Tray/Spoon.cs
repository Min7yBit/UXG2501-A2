using UnityEngine;

public class Spoon : MonoBehaviour, IInteractable
{
    public string Name => name;

    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;
    public bool interactable;
    private Renderer Rrenderer;
    private bool mouseOver = false;
    [SerializeField]private PlayerMovement playerMovement;
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
    private void OnMouseOver()
    {
        if (!interactable || !mouseOver)
            return;

        Debug.Log("Interacted with " + name);

        Item item = GetComponent<Item>();
        if (item != null && Input.GetMouseButtonDown(0))
        {
            playerMovement.GetComponent<Inventory>().AddItem(item); //testing adding item to inventory on interact, some items may not have Item component
            gameObject.SetActive(false); //disable the object after picking it up
        }
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
        //nothing
    }
}
