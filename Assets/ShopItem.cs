using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    string name;
    int price, quantity;
    public int index;

    void Start()
    {
        quantity = 0;
        UpdateQuantityLabel();
    }

    public void IncreaseQuantity()
    {
        if (!canClick()) return;

        quantity++;
        UpdateQuantityLabel();
    }

    public void DecreaseQuantity()
    {
        quantity--;
        if (quantity < 0) quantity = 0;
        UpdateQuantityLabel();
    }

    void UpdateQuantityLabel()
    {
        transform.Find("itemQtyLabel").GetComponent<TextMeshProUGUI>().text = "Qty: " + quantity;

        GameObject shopSystem = GameObject.Find("shopSystem");
        shopSystem.GetComponent<ShopSystem>().UpdateTotal(index, quantity);
    }

    bool canClick()
    {
        GameObject shopSystem = GameObject.Find("shopSystem");
        return shopSystem.GetComponent<ShopSystem>().CanAddItemsToCart(index);
    }

    void Update()
    {
    }
}
