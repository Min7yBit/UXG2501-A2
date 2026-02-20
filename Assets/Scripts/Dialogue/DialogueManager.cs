using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialogueBody;
    public GameObject dialogueChoicesParent;
    public Image dialoguePortrait;
    public TMP_Text characterName;
    public TMP_Text dialogueText;
    public bool isDialogueActive {get ; private set;}

    [SerializeField] private KeyCode closeKey = KeyCode.Escape;
    [SerializeField] private KeyCode nextKey = KeyCode.E;
    private bool close => Input.GetKey(closeKey);
    private bool next => Input.GetKey(nextKey);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!isDialogueActive && Input.GetKey(KeyCode.P) == true)
        {
            ShowDialogue("Name 1", "Dialogue Text 1", null);
        }

        if (isDialogueActive && close)
        {
            HideDialogue();
        }

        if (isDialogueActive && next)
        {
            Next("Name 2", "Dialogue Text 2", null);
        }

    }

    public void ShowDialogue(string name, string dialogue, Sprite portrait = null)
    {
        Debug.Log("Showing dialogue");
        isDialogueActive = true;
        dialogueBody.SetActive(true);
        characterName.text = name;
        dialogueText.text = dialogue;
        dialoguePortrait.sprite = portrait;
    }

    public void HideDialogue()
    {
        Debug.Log("Hiding dialogue");
        isDialogueActive = false;
        dialogueBody.SetActive(false);
        characterName.text = "";
        dialogueText.text = "";
        dialoguePortrait.sprite = null;
    }

    public void Next(string name, string dialogue, Sprite portrait = null)
    {
        Debug.Log("Next button pressed");
    }

}
