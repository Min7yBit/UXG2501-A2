using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private float raycastDistance = 3f;
    [SerializeField] private PlayerMovement playerMovement;

    private ObjectRotation currentTarget;

    void Start()
    {
        camera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, raycastDistance))
        {
            ObjectRotation rotatable = hit.collider.GetComponent<ObjectRotation>();

            if (rotatable != null)
            {
                Debug.Log("Hit Rotatable");
                if (currentTarget != rotatable)
                {
                    if (currentTarget != null)
                        currentTarget.rotateAllowed = false;

                    currentTarget = rotatable;
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {

                    currentTarget.rotateAllowed = true;
                    playerMovement.CanMove = false;
                }
                else
                {
                    ClearTarget();
                }
            }
            else
            {
                ClearTarget();
            }
        }
        else
        {
            ClearTarget();
        }
    }

    private void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.rotateAllowed = false;
            currentTarget = null;
            playerMovement.CanMove = true;
        }
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