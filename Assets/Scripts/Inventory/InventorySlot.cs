using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]private Inventory inventory;
    public Item currentItem; // The item in this slot
    public Image icon;       // UI Image showing the item
/*
    public Texture2D cursorEnterTexture;   // Custom cursor for when the mouse enters the slot
    public Texture2D cursorExitTexture;    // Custom cursor for when the mouse exits the slot
    public Vector2 cursorHotspotOffset = Vector2.zero; // Hotspot offset for the cursor*/
    public bool isSelected = false;
    private void Awake()
    {
    }

    public void SetItem(Item item)
    {
        currentItem = item;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    /*    // Called when the player clicks the slot
        public void OnClick()
        {
            if (currentItem != null)
            {
                if (isSelected)
                {
                    isSelected = false;
                    SetUnselectedUI();
                }
                else
                {
                    isSelected = true;
                    SetSelectedUI();
                }
    *//*            Debug.Log("Clicked on item: " + currentItem.itemName);
                if (currentItem is ICombinable combinableItem)
                {
                    CombinableItem combineItem = currentItem as CombinableItem;
                    CombineSystem combineSystem = inventory.combineSystem;
                    Debug.Log("Item " + combineItem.itemName + " is combinable.");
                    Debug.Log("Current AddedToCombine state: " + combineItem.AddedToCombine);
                    // Toggle the item's presence in the combine slots UI
                    if (combineItem.AddedToCombine)
                    {                    
                        Debug.Log("Removing item " + currentItem.itemName + " from combine slots.");
                        combineSystem.RemoveFromCombineSlot(combinableItem);

                        // Set transparency to indicate it's removed from the combine slot, had to use index because unity getchild searches parent as well tf
                        Image img = transform.GetChild(0).GetComponent<Image>();
                        Color currentColor = img.color;
                        currentColor.a = 1f;
                        img.color = currentColor;
                    }
                    else if (!combineSystem.full)
                    {                 
                        Debug.Log("Adding item " + currentItem.itemName + " to combine slots.");
                        combineSystem.AddToCombineSlot(combinableItem);

                        // Set transparency to indicate it's in the combine slot, had to use index because unity getchild searches parent as well tf
                        Image img = transform.GetChild(0).GetComponent<Image>();
                        Color currentColor = img.color;
                        currentColor.a = 100 / 255f;
                        img.color = currentColor;
                    }
                }
                else
                {
                    Debug.Log("Item " + currentItem.itemName + " is not combinable.");
                }*//*
            }
        }*/

    /*    // Called when the mouse enters the slot
        public void OnCursorEnter()
        {
            if (cursorEnterTexture != null && currentItem != null)
            {
                Cursor.SetCursor(cursorEnterTexture, cursorHotspotOffset, CursorMode.Auto);
            }
        }

        // Called when the mouse exits the slot
        public void OnCursorExit()
        {
            if (cursorExitTexture != null)
            {
                Cursor.SetCursor(cursorExitTexture, cursorHotspotOffset, CursorMode.Auto);
            }
            else
            {
                // Reset to default cursor if no custom exit texture
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }*/

    public void SelectToggle()
    {
        if (isSelected)
        {
            isSelected = false;
            SetUnselectedUI();
        }
        else
        {
            isSelected = true;
            SetSelectedUI();
        }
    }

    private void SetSelectedUI()
    {
        // Set it red and fully opaque
        Image img = GetComponent<Image>();
        Color currentColor = Color.red;
        currentColor.a = 1f;
        img.color = currentColor;
    }

    private void SetUnselectedUI()
    {
        //set it white and semi transparent
        Image img = GetComponent<Image>();
        Color currentColor = Color.white;
        currentColor.a = 100 / 255f;
        img.color = currentColor;
    }
}
