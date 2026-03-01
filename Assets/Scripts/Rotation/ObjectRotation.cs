using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public bool rotateAllowed;
    [SerializeField] private float speed = 100f;
    [SerializeField] private bool inverted;

    void Update()
    {
        if (!rotateAllowed)
            return;

        float mouseDeltaX = Input.GetAxis("Mouse X");
        mouseDeltaX *= speed * Time.deltaTime;

        transform.Rotate(Vector3.up * (inverted ? -1 : 1), mouseDeltaX, Space.World);
    }
}
