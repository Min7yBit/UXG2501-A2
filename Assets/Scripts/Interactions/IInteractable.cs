using UnityEngine;

public interface IInteractable
{
    public string Name { get;}

    public bool CanInteract { get; set; }
    public bool InInteract { get; set; }

    public bool ShowPrompt { get; set; }
    public void OnInteract(in PlayerMovement playerMovement);
    public Transform GetTransform();

    
}
