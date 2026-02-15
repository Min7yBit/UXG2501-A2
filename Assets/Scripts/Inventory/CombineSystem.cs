/*using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombineSystem : MonoBehaviour
{
    // This is the item to be combined
    private CombinableItem itemA;
    private CombinableItem itemB;    
    public Item combinedItem { get; private set; }
    public bool full { get; private set; } = false;
    public bool readyToCombine { get; private set; } = false;

    //to be filled up in inspector
    [SerializeField] private List<ItemRecipe> itemRecipes;
    [SerializeField] private TMP_Text text;

    // UI Elements
    public GameObject itemSlotA;
    public GameObject itemSlotB;
    public GameObject combinedSlot;

    public void AddToCombineSlot(ICombinable item)
    {
        if (itemA == null)
        {
            itemA = item as CombinableItem;
            itemA.AddedToCombine = true;
        }
        else if (itemB == null)
        {
            itemB = item as CombinableItem;
            itemB.AddedToCombine = true;
        }
        DisplayCombinedItem();
        DisplayItemUI();
    }
    public void RemoveFromCombineSlot(ICombinable item)
    {
        if (itemA == item as CombinableItem)
        {
            itemA.AddedToCombine = false;
            readyToCombine = false;
            full = false;
            itemA = null;
            combinedItem = null;
        }
        else if (itemB == item as CombinableItem)
        {
            itemB.AddedToCombine = false;
            readyToCombine = false;
            full = false;
            itemB = null;
            combinedItem = null;
        }
        DisplayItemUI();
    }
    public void DisplayCombinedItem()
    {
        if (itemA == null || itemB == null)
        {
            text.text = "Both item slots must be filled to combine.";            
            return;
        }
        foreach (var recipe in itemRecipes)
        {
            if ((recipe.inputA.itemName == itemA.itemName && recipe.inputB.itemName == itemB.itemName) || (recipe.inputA.itemName == itemB.itemName && recipe.inputB.itemName == itemA.itemName))
            {
                combinedItem = recipe.result;
                text.text = $"You can use {itemA.itemName} and {itemB.itemName} to create {combinedItem.itemName}!";
                readyToCombine = true;
                full = true;
                DisplayItemUI();
                return;
            }
        }
        full = true;
        text.text = "No valid combination found for the selected items.";
    }
    private void DisplayItemUI()
    {
        if (itemA != null)
        {
            Debug.Log($"Item A: {itemA.itemName}");
            itemSlotA.GetComponent<UnityEngine.UI.Image>().sprite = itemA.icon;
        }
        else if (itemA == null)
        {
            itemSlotA.GetComponent<UnityEngine.UI.Image>().sprite = null;
        }

        if (itemB != null)
        {
            Debug.Log($"Item B: {itemB.itemName}");
            itemSlotB.GetComponent<UnityEngine.UI.Image>().sprite = itemB.icon;
        }
        else if (itemB == null)
        {
            itemSlotB.GetComponent<UnityEngine.UI.Image>().sprite = null;
        }

        if (combinedItem != null)
        {
            Debug.Log($"Combined Item: {combinedItem.itemName}");
            combinedSlot.GetComponent<UnityEngine.UI.Image>().sprite = combinedItem.icon;
        }
        else if (combinedItem == null)
        {
            combinedSlot.GetComponent<UnityEngine.UI.Image>().sprite = null;
        }
    }

    public void ResetCombineSystem()
    {
        if (itemA != null)
        {
            itemA.AddedToCombine = false;
            itemA = null;
        }
        if (itemB != null)
        {
            itemB.AddedToCombine = false;
            itemB = null;
        }
        combinedItem = null;
        readyToCombine = false;
        full = false;
        text.text = "";
        DisplayItemUI();
    }
}

[System.Serializable]
public class ItemRecipe
{
    public Item inputA;
    public Item inputB;
    public Item result;
}*/