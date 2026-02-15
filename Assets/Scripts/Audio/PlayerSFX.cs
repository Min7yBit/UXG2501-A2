using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("SFX Clips")]
    public AudioClip[] footstepClips;
    public AudioClip jumpClip;
    public AudioClip landClip;

    [Header("Settings")]
    public float footstepInterval = 0.45f;
    public float footstepVolume = 0.8f;

    private float footstepTimer;
    private bool wasGrounded = true;

    private PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        bool grounded = movement.IsGrounded();

        // -------------------------
        // LANDING SFX
        // -------------------------
        if (!wasGrounded && grounded)
        {
            PlayOneShot(landClip, 0.9f);
        }
        wasGrounded = grounded;

        // -------------------------
        // FOOTSTEPS
        // -------------------------
        if (grounded && movement.IsMoving())
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // reset when not walking
        }
    }

    public void PlayJump()
    {
        PlayOneShot(jumpClip, 1f);
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        PlayOneShot(clip, footstepVolume);
    }

    private void PlayOneShot(AudioClip clip, float volume)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
