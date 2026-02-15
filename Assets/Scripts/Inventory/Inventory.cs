using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private List<Item> inventoryList;
    private List<InventorySlot> inventorySlots;
    [SerializeField] private int maxInventorySize = 8;
    [SerializeField] Transform invSlotTransform;
    [SerializeField] Transform inventoryUIParent;
    [SerializeField] private int currentSelectedSlotIndex = 0;

    [Header("Pickup SFX")]
    public AudioSource audioSource;
    public AudioClip pickupSFX;
    [Range(0f, 2f)] public float pickupVolume = 1f;

    private void Awake()
    {
        inventoryList = new List<Item>();
        inventorySlots = new List<InventorySlot>();
    }

    private void Start()
    {
        foreach (Transform child in invSlotTransform)
        {
            InventorySlot slot = child.GetComponent<InventorySlot>();
            if (slot != null)
                inventorySlots.Add(slot);
        }

        // Initialize first slot selected
        inventorySlots[currentSelectedSlotIndex].SelectToggle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentSelectedSlotIndex >= maxInventorySize - 1)
            {
                inventorySlots[currentSelectedSlotIndex].SelectToggle();
                currentSelectedSlotIndex = 0;
                inventorySlots[currentSelectedSlotIndex].SelectToggle();
            }
            else
            {
                inventorySlots[currentSelectedSlotIndex++].SelectToggle();
                inventorySlots[currentSelectedSlotIndex].SelectToggle();
            }
        }
    }

    public void AddItem(Item newItem)
    {
        if (inventoryList.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        inventoryList.Add(newItem);
        Debug.Log($"Added {newItem.itemName} to inventory.");

        // -----------------------------------
        // PLAY PICKUP SOUND HERE
        // -----------------------------------
        PlayPickupSFX();

        RefreshUI();
    }

    public void RemoveItem(Item itemToRemove)
    {
        if (inventoryList.Remove(itemToRemove))
        {
            Debug.Log($"Removed {itemToRemove.itemName} from inventory.");
            RefreshUI();
            return;
        }
        Debug.Log($"{itemToRemove.itemName} not found in inventory.");
    }

    private void RefreshUI()
    {
        Debug.Log("Refreshing Inventory UI...");

        foreach (var slot in inventorySlots)
            slot.SetItem(null);

        for (int i = 0; i < inventoryList.Count && i < inventorySlots.Count; i++)
            inventorySlots[i].SetItem(inventoryList[i]);

        inventorySlots[currentSelectedSlotIndex].SelectToggle();
        currentSelectedSlotIndex = 0;
        inventorySlots[currentSelectedSlotIndex].SelectToggle();
    }

    public Item GetItem(string name)
    {
        foreach (Item item in inventoryList)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }

    public bool ContainsItem(string name)
    {
        foreach (InventorySlot i in inventorySlots)
        {
            if (i.currentItem == null)
                continue;

            if (i.isSelected && i.currentItem.itemName == name)
                return true;
        }
        return false;
    }

    // -------------------------------------------------------
    // PLAY PICKUP SFX
    // -------------------------------------------------------
    private void PlayPickupSFX()
    {
        if (audioSource != null && pickupSFX != null)
            audioSource.PlayOneShot(pickupSFX, pickupVolume);
    }
}
