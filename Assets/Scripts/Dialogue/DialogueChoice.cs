using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    public Sprite unselected;
    public Sprite selected;
    public Image choiceImage;
    public TMP_Text choiceText;

    public void SetChoiceText(string text)
    {
        choiceText.text = text;
    }
    
    public void SetSelected()
    {
        choiceImage.sprite = selected;
        choiceText.text = "<mark=#FF000080><u>" + choiceText.text + "</u></mark>"; // Make the text underline when selected 
    }

    public void SetUnselected()
    {
        choiceImage.sprite = unselected;
        choiceText.text = choiceText.text.Replace("<u>", "").Replace("</u>", "").Replace("<mark=#FF000080>", "").Replace("</mark>", ""); ; // Remove underline from text when unselected
    }
}
