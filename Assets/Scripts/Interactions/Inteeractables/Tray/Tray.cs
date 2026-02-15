using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour, IInteractable
{
    public string Name => name;
    public bool CanInteract { get => interactable; set { interactable = value; } }
    public bool ShowPrompt { get; set; } = true;
    public bool InInteract { get; set; } = false;

    [Header("Camera")]
    public CameraControl cameraControl;
    public Camera cam;

    [Header("Potato Logic")]
    public Potato potato;

    [Header("Tray Interaction SFX")]
    public AudioSource audioSource;     // assign in inspector
    public AudioClip trayOpenSFX;       // assign sound for opening the tray
    [Range(0f, 2f)] public float trayOpenVolume = 1f;

    private bool interactable = true;
    private bool mouseOver = false;
    private PlayerMovement playerMovement;
    [SerializeField] private List<Transform> childTransforms;

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

        //  PLAY INTERACTION SFX
        PlayTraySFX();

        EnableChildInteraction();
        interactable = false;
        InInteract = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.playerMovement = playerMovement;
        this.playerMovement.CanMove = false;

        cameraControl.SwitchToFixedCamera(cam);
    }

    private void Update()
    {
        if (InInteract)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                InInteract = false;

                if (potato != null)
                    potato.ResetPotato();

                DisableChildInteraction();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                interactable = true;
                playerMovement.CanMove = true;

                cameraControl.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
            }
        }
    }

    // ---------------------
    // AUDIO
    // ---------------------
    private void PlayTraySFX()
    {
        if (audioSource != null && trayOpenSFX != null)
        {
            audioSource.PlayOneShot(trayOpenSFX, trayOpenVolume);
        }
        else
        {
            Debug.LogWarning("[Tray] Missing AudioSource or trayOpenSFX!");
        }
    }

    // ---------------------
    // INTERACTION LOGIC
    // ---------------------
    private void InitiateTransformList()
    {
        childTransforms = new List<Transform>();
        Transform[] newTransformList = transform.GetComponentsInChildren<Transform>();

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
            {
                interactableComponent.CanInteract = false;
            }
        }
    }

    private void EnableChildInteraction()
    {
        foreach (Transform child in childTransforms)
        {
            IInteractable interactableComponent = child.GetComponent<IInteractable>();
            if (interactableComponent != null)
            {
                interactableComponent.CanInteract = true;
            }
        }
    }
}
