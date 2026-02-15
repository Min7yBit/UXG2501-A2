using UnityEngine;

public class GlobalSFX : MonoBehaviour
{
    public static GlobalSFX Instance;

    private AudioSource audioSource;

    public AudioClip uiClickSFX;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayUIClick()
    {
        if (uiClickSFX != null)
            audioSource.PlayOneShot(uiClickSFX);
    }
}
