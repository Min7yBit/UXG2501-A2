using UnityEngine;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{

    [Header("Target Transforms")]
    public Transform pointA;
    public Transform pointB;

    [Header("Settings")]
    public float speed = 2f;
    public bool lerpRotation = true;
    public KeyCode triggerKey = KeyCode.Space;

    private float _t = 0f;
    private bool isPlaying = false;
    private bool _goingForward = true;

    public PlayerMovement playerMovement;
    public ObjectRotationController rotationController;

    void Start()
    {
        transform.SetParent(pointA);
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            _goingForward = !_goingForward;
            isPlaying = true;
            transform.SetParent(null); // unparent while moving
        }

        if (!isPlaying) return;

        _t = Mathf.MoveTowards(_t, _goingForward ? 1f : 0f, Time.deltaTime * speed);

        transform.position = Vector3.Lerp(pointA.position, pointB.position, _t);

        if (lerpRotation)
            transform.rotation = Quaternion.Slerp(pointA.rotation, pointB.rotation, _t);

        if (_t >= 1f)
        {
            isPlaying = false;
            transform.SetParent(pointB); // child of B on arrival
            playerMovement.CanMove = false;
            rotationController.canRotate = true;
        }
        else if (_t <= 0f)
        {
            isPlaying = false;
            transform.SetParent(pointA); // child of A on arrival
            playerMovement.CanMove = true;
            rotationController.canRotate = false;
        }
    }


    //// --------------------
    //// Audio Helper
    //// --------------------
    //private void PlaySFX(AudioClip clip, float volume)
    //{
    //    if (audioSource != null && clip != null)
    //        audioSource.PlayOneShot(clip, volume);
    //}
}
