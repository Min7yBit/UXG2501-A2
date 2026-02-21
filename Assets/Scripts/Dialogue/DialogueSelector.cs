using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSelector : MonoBehaviour
{
    private int index = 0;
    private DialogueRoot currentDialogueData;
    private List<DialogueChoice> dialogueChoices;
    private List<string> dialogueChoicesTextList;
    private List<string> dialogueChoicesIdList;
    //public bool changingChoices = false;
    private bool dialogueActive => DialogueManager.Instance.isDialogueActive;
    [SerializeField] private KeyCode upKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode downKey = KeyCode.DownArrow;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    private bool up => Input.GetKeyDown(upKey);
    private bool down => Input.GetKeyDown(downKey);

    public GameObject dialogueChoicePrefab;

    private void Awake()
    {
        dialogueChoices = new List<DialogueChoice>();
        dialogueChoicesTextList = new List<string>();
        dialogueChoicesIdList = new List<string>();        
    }
    private void Start()
    {
        DialogueManager.Instance.dialogueBody.SetActive(false);
    }
    private void Update()
    {
        if(dialogueActive && up)
        {
            MoveUpChoice();
        }
        if(dialogueActive && down)
        {
            MoveDownChoice();
        }
        if (Input.GetKeyDown(KeyCode.O) == true)
        {
            Debug.Log(dialogueChoices.Count);
        }
    }

    public void AddDialogueChoice(string[] dialogueChoiceTextArray, string[] dialogueChoiceIdArray, DialogueRoot dialogueData)
    {     
        dialogueChoicesTextList.Clear();        
        dialogueChoicesIdList.Clear();
        currentDialogueData = dialogueData;
        dialogueChoicesTextList.AddRange(dialogueChoiceTextArray);        
        dialogueChoicesIdList.AddRange(dialogueChoiceIdArray);        
        SpawnDialogueChoices();
    }

    private void SpawnDialogueChoices()
    {
        ResetScrollToTop();

        if (dialogueChoicesTextList.Count == 0) // No choices to spawn
            return;
        
        foreach (string dialogueChoiceText in dialogueChoicesTextList)
        {
            GameObject choice = Instantiate(dialogueChoicePrefab, transform);
            dialogueChoices.Add(choice.GetComponent<DialogueChoice>());
            choice.GetComponent<DialogueChoice>().SetChoiceText(dialogueChoiceText);
        }
        dialogueChoices[0].SetSelected();

        ResetScrollToTop();
    }
    public void ClearDialogueChoices()
    {
        currentDialogueData = null;
        foreach (DialogueChoice choice in dialogueChoices)
        {
            Destroy(choice.gameObject);
        }
        dialogueChoices.Clear();
        dialogueChoicesTextList.Clear();
        dialogueChoicesIdList.Clear();
        index = 0;
    }
    public void NextChoice()
    {
        if (dialogueChoicesTextList.Count == 0) // No choices to, just exit
        {
            DialogueManager.Instance.HideDialogue();
            return;
        }

        DialogueEntry nextDialogue = FindNextDialogue(dialogueChoicesIdList[index]);
        if (nextDialogue == null)
        {
            Debug.LogError("No dialogue found for choice ID: " + dialogueChoicesIdList[index]);
            return;
        }
        DialogueManager.Instance.Next(nextDialogue.characterName, nextDialogue.dialogueText, nextDialogue.dialogueChoices, nextDialogue.dialogueChoicesId, currentDialogueData);
    }
    private DialogueEntry FindNextDialogue(string dialogueId)
    {
        DialogueEntry nextDialogue = null;
        for (int i = 0; i< currentDialogueData.allDialogues.Length; i++)
        {
            if (currentDialogueData.allDialogues[i].dialogueId == dialogueId)
            {
                nextDialogue = currentDialogueData.allDialogues[i];
                break;
            }
        }

        return nextDialogue;
    }
    private void MoveUpChoice()
    {
        Debug.Log("Up");
        if (dialogueChoices == null) // No choices to move through
            return;
        if (dialogueChoices.Count == 0) // No choices to move through
            return;

        if (index <= 0) // Already at the top of the list, can't move up
        {
            index = 0;
            return;
        }
        else
        {
            dialogueChoices[index].SetUnselected();
            index--;
            dialogueChoices[index].SetSelected();
            ScrollToSelected();
        }
    }

    private void MoveDownChoice()
    {
        Debug.Log("Down");
        if (dialogueChoices == null) // No choices to move through
            return;
        if (dialogueChoices.Count == 0) // No choices to move through
            return;

        if (index >= dialogueChoices.Count - 1) // Already at the bottom of the list, can't move up
        {
            index = dialogueChoices.Count - 1;
            return;
        }
        else
        {
            dialogueChoices[index].SetUnselected();
            index++;
            dialogueChoices[index].SetSelected();
            ScrollToSelected();
        }
    }
    private void ScrollToSelected()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform selected = dialogueChoices[index].GetComponent<RectTransform>();

        float contentHeight = content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        float selectedY = Mathf.Abs(selected.anchoredPosition.y);

        float normalizedPosition = 1 - Mathf.Clamp01(
            selectedY / (contentHeight - viewportHeight)
        );

        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }
    private void ResetScrollToTop()
    {
        Canvas.ForceUpdateCanvases();   // IMPORTANT
        scrollRect.verticalNormalizedPosition = 1f;
    }


}
