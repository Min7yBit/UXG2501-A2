using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject hintsFound;
    //public Inventory playerInventory;
    //public CombineSystem combineSystem;
    public int hintsCount = 0;

    private bool isPaused = false;

    void Update()
    {
        // ESC behaviour: close inventory first, pause second
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*            if (inventory.activeSelf)
                        {
                            ToggleInventory();
                            return;
                        }*/

            TogglePauseMenu();
        }

        /*        // I key (only allowed when not paused)
                if (Input.GetKeyDown(KeyCode.I) && !isPaused)
                {
                    ToggleInventory();
                }*/

    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
    public void ShowWinMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        winMenu.SetActive(true);
    }

    public void UpdateHintsCount()
    {
        if (hintsCount < 3)
            hintsCount++;

        hintsFound.GetComponentInChildren<TMP_Text>().text = $"Hints Found: {hintsCount}/3";
    }

    public void SetGameTime(bool active)
    {
        if (active)
        {
            {
                Time.timeScale = active ? 1 : 0;
            }
        }
    }
}
