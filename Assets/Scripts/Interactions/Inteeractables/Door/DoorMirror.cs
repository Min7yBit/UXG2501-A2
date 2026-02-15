using UnityEngine;

public class DoorMirror : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    public CameraControl cameraControl;
    public Camera cam;

    private bool interactable = true;
    private PlayerMovement playerMovement;
    [SerializeField] private Collider doorCol;
    [SerializeField] private WinCondition winCondition;
    public Transform GetTransform()
    {
        return transform;
    }
    public void OnInteract(in PlayerMovement playerMovement)
    {   
        if (!interactable) 
            return;
        winCondition.allowInteract = true;
        interactable = false;
        InInteract = true;
        Debug.Log("Interacted with " + name);
        this.playerMovement = playerMovement;
        this.playerMovement.CanMove = false;
        cameraControl.SwitchToFixedCamera(cam);        
        doorCol.enabled = false; //disables door collider
    }

    private void Update()
    {
        if (InInteract)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                winCondition.allowInteract = false;
                winCondition.ResetAllCombinations();
                InInteract = false;
                interactable = true;
                doorCol.enabled = true; //enables door collider again
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                interactable = true;
                playerMovement.CanMove = true;
                cameraControl.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
            }
        }
    }
}
