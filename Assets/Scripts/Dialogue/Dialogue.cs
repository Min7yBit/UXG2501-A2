using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string jsonFileName = "Level1_";
    private DialogueRoot dialogueData;
    private bool dialogueActive => DialogueManager.Instance.isDialogueActive;
    private void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueRoot>(jsonFile.text);
        }
    }

    public void InitialiseDialogue()
    {
        //always show first dialogue during intialisation
        if (dialogueData == null)
            return;
        if (dialogueActive)
            return;

        DialogueEntry firstDialogue = dialogueData.allDialogues[0];
        DialogueManager.Instance.ShowDialogue(firstDialogue.characterName, firstDialogue.dialogueText, firstDialogue.dialogueChoices, firstDialogue.dialogueChoicesId, dialogueData);
    }

}

[System.Serializable]
public class DialogueEntry // Matches the "Object" { }
{
    public string dialogueId;
    public string characterName;
    public string dialogueText;
    public string[] dialogueChoices;
    public string[] dialogueChoicesId;
}

[System.Serializable]
public class DialogueRoot // The Wrapper to hold the [ ]
{
    public DialogueEntry[] allDialogues;
}
