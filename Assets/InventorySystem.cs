using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private List<Item> playerInventory;
    private int currentInventoryIndex = 0;
    private bool isVisible = false;

    [SerializeField] private GameObject PickupUI;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryText;
    [SerializeField] private GameObject inventoryDescription;
    [SerializeField] private GameObject inventoryImage;

    void Start()
    {
        playerInventory = new List<Item>
        {
            new Item(Item.ItemType.MEAT),
            new Item(Item.ItemType.GOLD)
        };
        playerInventory[1].nb = 300;

        CheckInventory();
        DisplayUI(false);

        if (PickupUI != null)
            PickupUI.SetActive(false);
    }

    void Update()
    {
        if (isVisible)
        {
            DisplayUI(true);

            Item currentItem = playerInventory[currentInventoryIndex];

            if (inventoryText.TryGetComponent(out TextMeshProUGUI textComponent))
                textComponent.text = $"{currentItem.name} [{currentItem.nb}]";

            if (inventoryDescription.TryGetComponent(out TextMeshProUGUI descriptionComponent))
                descriptionComponent.text = currentItem.description + "\n\n Press [U] to Select";

            if (inventoryImage.TryGetComponent(out RawImage imageComponent))
                imageComponent.texture = currentItem.GetTexture();

            if (Input.GetKeyDown(KeyCode.I))
                currentInventoryIndex++;

            if (currentInventoryIndex >= playerInventory.Count)
            {
                currentInventoryIndex = 0;
                isVisible = false;
                DisplayUI(false);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                if (currentItem.familyType == Item.ItemFamilyType.FOOD)
                {
                    GetComponent<ControlPlayer>()?.IncreaseHealth(currentItem.healthBenefits);
                    playerInventory.RemoveAt(currentInventoryIndex);

                    currentInventoryIndex = 0;
                    isVisible = false;
                    DisplayUI(false);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            isVisible = true;
        }
    }

    void DisplayUI(bool toggle)
    {
        if (inventoryText != null) inventoryText.SetActive(toggle);
        if (inventoryPanel != null) inventoryPanel.SetActive(toggle);
        if (inventoryImage != null) inventoryImage.SetActive(toggle);
        if (inventoryDescription != null) inventoryDescription.SetActive(toggle);
    }
    public void AddPurchasedItems(List<Item> purchasedItems)
    {
        bool t;
        for (int i = 0; i < purchasedItems.Count; i++) { if (purchasedItems[i].nb > 0) t = UpdateItem(purchasedItems[i].type, purchasedItems[i].nb); }
    }

    void CheckInventory()
    {
        foreach (var item in playerInventory)
        {
            Debug.Log(item.ItemInfo());
        }
    }
    public int GetMoney() 
    { 
        for (int i = 0; i < playerInventory.Count; i++) 
        { 
            if (playerInventory[i].type == Item.ItemType.GOLD) 
            { return (playerInventory[i].nb); } } 
        return 0; 
    }


    public void SetMoney(int newAmount) {
        for (int i = 0; i < playerInventory.Count; i++) {
            if (playerInventory[i].type == Item.ItemType.GOLD) 
            { playerInventory[i].nb = newAmount; } } }


    public bool UpdateItem(Item.ItemType type, int nbItemsToAdd)
    {
        foreach (var item in playerInventory)
        {
            if (item.type == type)
            {
                if (item.nb + nbItemsToAdd <= item.maxNb)
                {
                    item.nb += nbItemsToAdd;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        var newItem = new Item(type) { nb = nbItemsToAdd };
        playerInventory.Add(newItem);
        return true;
    }
}
