using System.Collections.Generic;
using System.Data;
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

    private Camera camera;
    [SerializeField] private float raycastDistance = 3f;

    private PlayerMovement playerMovement;


    private void Start()
    {
        camera = Camera.main;
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

        IInteractable currentInteractable = null;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, raycastDistance))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();

        }

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
        foreach (IInteractable interactable in interactableList)
        {
            if (currentInteractable == interactable) // Setting the outline
            {
                interactable.GetTransform().gameObject.layer = 6; // 6 is "Outline" Layer index
                SetLayerRecursively(interactable.GetTransform().gameObject, 6);
                Debug.Log("Outline");
            }
            else 
            {
                interactable.GetTransform().gameObject.layer = 0; // Set Default layer
                SetLayerRecursively(interactable.GetTransform().gameObject, 0);

            }
        }

        if (currentInteractable != null)
        {
            return currentInteractable;
        }

        return null;

    }

    private void SetLayerRecursively(GameObject parentObject, int LayerIndex)
    {

        if (LayerIndex == -1)
        {
            Debug.LogError(" does not exist. Make sure to define it in the Unity Editor.");
            return;
        }

        // Use GetComponentsInChildren to get all transforms in the hierarchy (including the parent)
        foreach (Transform trans in parentObject.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = LayerIndex ;
        }
    }

    /// <summary>
    /// this is only for editor to visualise the interact radius
    /// </summary>
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    private void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * raycastDistance);
        Gizmos.DrawWireSphere(cam.transform.position + cam.transform.forward * raycastDistance, 0.05f);
    }
}
