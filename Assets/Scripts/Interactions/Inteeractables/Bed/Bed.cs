using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool InInteract { get; set; } = false;
    public bool ShowPrompt { get; set; } = true;

    [Header("Camera")]
    public CameraControl cameraControl;
    public Camera cam;

    [Header("Bed Enter SFX")]
    public AudioSource audioSource;          // assign in Inspector
    public AudioClip bedEnterSFX;           // sound when clicking into bed
    [Range(0f, 2f)] public float bedEnterVolume = 1f;

    private bool interactable = true;
    private bool mouseOver = false;
    private PlayerMovement playerMovement;
    [SerializeField] private List<Transform> childTransforms;
    [SerializeField] private Transform playerLocation;

    private void Awake()
    {
        InitiateTransformList();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    private void OnMouseEnter()
    {
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        mouseOver = false;
    }
    public void OnInteract(in PlayerMovement playerMovement)
    {
        if (!mouseOver) return;

        GetComponent<Collider>().enabled = false;
        //  play bed-enter sound when clicking into bed
        PlayBedEnterSFX();

        InInteract = true;
        EnableChildInteraction();
        interactable = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.playerMovement = playerMovement;
        this.playerMovement.CanMove = false;

        // move player to correct position beside bed
        this.playerMovement.transform.position =
            new Vector3(playerLocation.position.x,
                        this.playerMovement.transform.position.y,
                        playerLocation.position.z);

        cameraControl.SwitchToFixedCamera(cam);
    }

    private void Update()
    {
        if (InInteract)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                GetComponent<Collider>().enabled = true;
                InInteract = false;
                DisableChildInteraction();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                interactable = true;
                playerMovement.CanMove = true;
                cameraControl.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
            }
        }
    }

    // ---------------------------
    // SFX
    // ---------------------------
    private void PlayBedEnterSFX()
    {
        if (audioSource != null && bedEnterSFX != null)
        {
            audioSource.PlayOneShot(bedEnterSFX, bedEnterVolume);
        }
        else
        {
            Debug.LogWarning("[Bed] Missing AudioSource or bedEnterSFX!");
        }
    }

    // ---------------------------
    // Child interaction helpers
    // ---------------------------
    private void InitiateTransformList()
    {
        childTransforms = new List<Transform>();
        Transform[] newTransformList = transform.GetComponentsInChildren<Transform>();

        // start at 1 to skip the parent transform
        for (int i = 1; i < newTransformList.Length; i++)
        {
            childTransforms.Add(newTransformList[i]);
        }

        DisableChildInteraction();
    }

    private void DisableChildInteraction()
    {
        foreach (Transform child in childTransforms)
        {
            IInteractable interactableComponent = child.GetComponent<IInteractable>();
            if (interactableComponent != null)
                interactableComponent.CanInteract = false;
        }
    }

    private void EnableChildInteraction()
    {
        foreach (Transform child in childTransforms)
        {
            IInteractable interactableComponent = child.GetComponent<IInteractable>();
            if (interactableComponent != null)
                interactableComponent.CanInteract = true;
        }
    }
}
