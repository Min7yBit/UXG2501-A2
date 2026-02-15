using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private float interactRange;

    [SerializeField] private TextMeshProUGUI interactPrompt;
    [SerializeField] private GameObject interactPromptGO;
    [SerializeField] private Image interactPrompImage;
    [SerializeField] private LayerMask interactLayerMask;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>(); 
    }

    private void Update()
    {

        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            if (interactable.ShowPrompt && !interactable.InInteract)
            {
                // UI to appear to show can interact
                if (interactPromptGO != null)
                {
                    interactPrompt.text = "Press LMB to interact with " + interactable.Name;
                    interactPromptGO.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                interactable.OnInteract(playerMovement);
            }

        }
        else
        {
            if (interactPromptGO != null)
            {
                // Set the UI to dissapear
                interactPromptGO.SetActive(false);
            }
        }
    }
    
    private IInteractable GetInteractableObject() // use to search for any interactable objects nearby and to find the nearest one 
    {
        List<IInteractable> interactableList = new();
        
        // Get all the colliders within interaction range with the layer mask of interaction
        Collider[] colliderarray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderarray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.CanInteract)
                    interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
            {
                // Closer
                closestInteractable = interactable;
                
            }
        }
        return closestInteractable;
    }
   

    /// <summary>
    /// this is only for editor to visualise the interact radius
    /// </summary>
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
