using UnityEngine;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    public enum CameraMode { FirstPerson, ThirdPerson, Fixed }
    public CameraMode currentMode = CameraMode.ThirdPerson;

    public List<Camera> thirdPersonCameras = new List<Camera>();

    public Camera firstPersonCam;
    public Camera thirdPersonCam;
    public Camera activeFixedCam;
    public Camera cellFixedCam;

    [Header("Camera Switch SFX")]
    public AudioSource audioSource;

    public AudioClip fpvEnterSFX;
    [Range(0f, 2f)] public float fpvEnterVolume = 1f;

    public AudioClip tppEnterSFX;
    [Range(0f, 2f)] public float tppEnterVolume = 1f;

    public AudioClip fixedEnterSFX;
    [Range(0f, 2f)] public float fixedEnterVolume = 1f;

    void Start()
    {
        SetCameraMode(currentMode);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCameraMode(CameraMode.FirstPerson);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCameraMode(CameraMode.ThirdPerson);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToFixedCamera(cellFixedCam);
        }
    }

    public void SetCameraMode(CameraMode mode)
    {
        //  SFX When switching modes
        if (mode != currentMode)
        {
            if (mode == CameraMode.FirstPerson)
                PlaySFX(fpvEnterSFX, fpvEnterVolume);

            else if (mode == CameraMode.ThirdPerson)
                PlaySFX(tppEnterSFX, tppEnterVolume);

            else if (mode == CameraMode.Fixed)
                PlaySFX(fixedEnterSFX, fixedEnterVolume);
        }

        currentMode = mode;

        // Enable the correct cameras
        firstPersonCam.enabled = (mode == CameraMode.FirstPerson);
        thirdPersonCam.enabled = (mode == CameraMode.ThirdPerson);

        if (mode == CameraMode.Fixed && activeFixedCam != null)
        {
            activeFixedCam.enabled = true;
        }
        else
        {
            foreach (var cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (cam.CompareTag("FixedCamera"))
                    cam.enabled = false;
            }
        }
    }

    public void SwitchToFixedCamera(Camera newCam)
    {
        if (activeFixedCam != null)
            activeFixedCam.enabled = false;

        activeFixedCam = newCam;
        SetCameraMode(CameraMode.Fixed);
    }

    // --------------------
    // Audio Helper
    // --------------------
    private void PlaySFX(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
