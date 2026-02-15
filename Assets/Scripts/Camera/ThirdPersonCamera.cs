using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float followSpeed = 10f;
    public float rotateSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Smooth rotation to face the target
        Quaternion lookRot = Quaternion.LookRotation(target.position + Vector3.up * 1.5f - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);
    }
}
