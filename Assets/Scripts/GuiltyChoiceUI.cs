using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiltyChoiceUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public Button guiltyButton;
    public Button notGuiltyButton;
    public Button closeButton;
    public TMP_Text resultText;
    public bool canOpen = false;

    public GameObject UImanager;

    [Header("Case Settings")]
    public bool isGuiltyCorrect;

    void Start()
    {
        popupPanel.SetActive(false);
        canOpen = false;
        guiltyButton.onClick.AddListener(() => MakeChoice(true));
        notGuiltyButton.onClick.AddListener(() => MakeChoice(false));
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void OpenPopup(bool correctAnswer)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        popupPanel.SetActive(true);
        resultText.text = "";
        isGuiltyCorrect = correctAnswer;

        guiltyButton.interactable = true;
        notGuiltyButton.interactable = true;
    }

    void MakeChoice(bool choseGuilty)
    {
        bool isCorrect = (choseGuilty == isGuiltyCorrect);

        if (isCorrect)
        {
            resultText.text = "Correct! 🎉";
        }
        else
        {
            resultText.text = "Wrong! ❌";
        }

        guiltyButton.interactable = false;
        notGuiltyButton.interactable = false;
    }

    void Update()
    {
        if (UImanager.GetComponent<UIManager>().hintsCount >= 8)
            canOpen = true;

        if (Input.GetKeyDown(KeyCode.F) && canOpen)
            OpenPopup(true);
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}