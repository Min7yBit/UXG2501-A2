using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 3f;

    private ObjectRotation currentTarget;
    public bool canRotate;

    public LayerMask layerMask;

    private void Start()
    {
        canRotate = false;
    }

    void Update()
    {
        if (!canRotate) return;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance, layerMask))
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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * raycastDistance, 0.05f);
    }
}