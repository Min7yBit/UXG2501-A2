using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool allowInteract = false;
    public LockCombination lockCombination1;
    public LockCombination lockCombination2;
    public LockCombination lockCombination3;
    public UIManager manager;

    [Header("Win SFX")]
    public AudioSource audioSource;        // assign in inspector
    public AudioClip winSFX;               // assign win sound
    [Range(0f, 2f)] public float winVolume = 1f;

    private bool hasWon = false;           // prevents multiple triggers

    private void Update()
    {
        if (allowInteract)
        {
            lockCombination1.CanInteract = true;
            lockCombination2.CanInteract = true;
            lockCombination3.CanInteract = true;
        }
        else
        {
            lockCombination1.CanInteract = false;
            lockCombination2.CanInteract = false;
            lockCombination3.CanInteract = false;
        }
    }

    public void ResetAllCombinations()
    {
        lockCombination1.ResetCombination();
        lockCombination2.ResetCombination();
        lockCombination3.ResetCombination();
        hasWon = false;
    }

    public void CheckWinCondition()
    {
        // Already won? Don’t trigger again
        if (hasWon)
            return;

        if (lockCombination1.IsCorrectCombination() &&
            lockCombination2.IsCorrectCombination() &&
            lockCombination3.IsCorrectCombination())
        {
            Debug.Log("Win Condition Met! You unlocked the door!");

            //  PLAY WIN SFX
            PlayWinSFX();

            hasWon = true;

            // Show UI win menu
            manager.ShowWinMenu();
        }
    }

    private void PlayWinSFX()
    {
        if (audioSource != null && winSFX != null)
            audioSource.PlayOneShot(winSFX, winVolume);
        else
            Debug.LogWarning("[WinCondition] Missing audioSource or winSFX!");
    }
}
