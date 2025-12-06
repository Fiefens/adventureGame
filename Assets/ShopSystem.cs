using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    [System.NonSerialized]
    public List<Item> shopItems;

    public GameObject shopItemComponent;
    private GameObject[] shopItemComponents;
    bool shopInitialized = false;
    public GameObject shopItemsObj;

    public GameObject shopMoneyLeftLabel;
    public GameObject shopTotalValue;
    public GameObject shopMoneyLeftValue;
    public int moneyLeft;
    private int initialMoney;
    private int totalPurchase;

    private float topLeftX, topLeftY;

    private void Awake()
    {
        InitializeShopItems();
    }

    void Start()
    {
        Init();
        SetupAllShopItemComponents();
    }

    public void Init()
    {
        initialMoney = GameObject.Find("Player").GetComponent<InventorySystem>().GetMoney();  moneyLeft = initialMoney;
        moneyLeft = initialMoney;
        topLeftX = 50f;
        topLeftY = 363f;
    }

    private void InitializeShopItems()
    {
        shopItems = new List<Item>
        {
            new Item(Item.ItemType.YELLOW_DIAMOND),
            new Item(Item.ItemType.BLUE_DIAMOND),
            new Item(Item.ItemType.RED_DIAMOND),
            new Item(Item.ItemType.MEAT),
            new Item(Item.ItemType.APPLE)
        };

        shopItemComponents = new GameObject[shopItems.Count];
    }

    private void SetupAllShopItemComponents()
    {
        if (shopInitialized)
            return;

        shopMoneyLeftLabel.GetComponent<TextMeshProUGUI>().text = initialMoney.ToString();

        for (int i = 0; i < shopItems.Count; i++)
        {
            SetupShopItemComponent(i);
        }

        shopInitialized = true;
    }

    private void SetupShopItemComponent(int index)
    {
        shopItems[index].nb = 0;

        GameObject itemObj = Instantiate(shopItemComponent);
        itemObj.name = $"shopItem_{index}_{shopItems[index].name}";

        Transform shopItemsParent = shopItemsObj.transform;
        itemObj.transform.SetParent(shopItemsParent, false);

        ShopItem shopItemScript = itemObj.GetComponentInChildren<ShopItem>();
        shopItemScript.index = index;

        itemObj.transform.Find("itemLabel").GetComponent<TextMeshProUGUI>().text =
            $"{shopItems[index].name} (${shopItems[index].price})";
        itemObj.transform.Find("itemQtyLabel").GetComponent<TextMeshProUGUI>().text =
            shopItems[index].nb.ToString();
        itemObj.transform.Find("itemImage").GetComponent<RawImage>().texture =
            shopItems[index].GetTexture();

        RectTransform bgRect = itemObj.transform.Find("itemBg").GetComponent<RectTransform>();
        float width = bgRect.sizeDelta.x;
        float spacing = 1.05f;

        RectTransform itemRect = itemObj.GetComponent<RectTransform>();
        itemRect.anchoredPosition = new Vector2(
            topLeftX + (index % 3) * (width * spacing),
            topLeftY - (index / 3) * (width * spacing)
        );

        shopItemComponents[index] = itemObj;
    }

    public void UpdateTotal(int itemIndex, int itemAmount)
    {
        shopItems[itemIndex].nb = itemAmount;

        int tempTotal = CalculateTotal();
        totalPurchase = tempTotal;
        moneyLeft = initialMoney - tempTotal;

        shopTotalValue.GetComponent<TextMeshProUGUI>().text = tempTotal.ToString();
        shopMoneyLeftValue.GetComponent<TextMeshProUGUI>().text = moneyLeft.ToString();
    }

    public int CalculateTotal()
    {
        int temp = 0;
        for (int i = 0; i < shopItems.Count; i++)
        {
            temp += shopItems[i].nb * shopItems[i].price;
        }
        return temp;
    }

    public bool CanAddItemsToCart(int index)
    {
        return moneyLeft >= shopItems[index].price && shopItems[index].nb < shopItems[index].maxNb;
    }
}
