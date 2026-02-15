using UnityEngine;

public class FixedCameraTrigger : MonoBehaviour
{
    public Camera fixedCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControl camManager = other.GetComponent<CameraControl>();
            if (camManager != null)
                camManager.SwitchToFixedCamera(fixedCam);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControl camManager = other.GetComponent<CameraControl>();
            if (camManager != null)
                camManager.SetCameraMode(CameraControl.CameraMode.ThirdPerson);
        }
    }
}
