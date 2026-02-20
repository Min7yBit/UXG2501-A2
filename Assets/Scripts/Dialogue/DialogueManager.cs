using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialogueBody;
    public GameObject dialogueChoicesParent;
    public DialogueSelector dialogueSelector;
    public Image dialoguePortrait;
    public TMP_Text characterName;
    public TMP_Text dialogueText;
    public bool isDialogueActive {get ; private set;}

    [SerializeField] private KeyCode closeKey = KeyCode.Escape;
    [SerializeField] private KeyCode nextKey = KeyCode.E;
    private bool close => Input.GetKeyDown(closeKey);
    private bool next => Input.GetKeyDown(nextKey);

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
        if (isDialogueActive && close)
        {
            HideDialogue();
        }

        if (isDialogueActive && next)
        {
            dialogueSelector.NextChoice();
        }
    }

    public void ShowDialogue(string name, string dialogue, string[] dialogueChoices, string[] dialogueChoicesId, DialogueRoot dialogueData, Sprite portrait = null)
    {        
        isDialogueActive = true;
        characterName.text = name;
        dialogueText.text = dialogue;
        dialoguePortrait.sprite = portrait;        
        dialogueSelector.AddDialogueChoice(dialogueChoices, dialogueChoicesId, dialogueData);
        dialogueBody.SetActive(true);
    }

    public void HideDialogue()
    {
        isDialogueActive = false;
        dialogueBody.SetActive(false);
        characterName.text = "";
        dialogueText.text = "";
        dialoguePortrait.sprite = null;
        dialogueSelector.ClearDialogueChoices();
    }

    public void Next(string name, string dialogue, string[] dialogueChoices, string[] dialogueChoicesId, DialogueRoot dialogueData, Sprite portrait = null)
    {
        HideDialogue();
        ShowDialogue(name, dialogue, dialogueChoices, dialogueChoicesId, dialogueData, portrait);
    }
}
