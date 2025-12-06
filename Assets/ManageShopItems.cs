using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageShopItems : MonoBehaviour
{
    public void IncreaseQuantity()
    {
        print("Just Clicked");
        GetComponentInParent<ShopItem>().IncreaseQuantity();
    }

    public void DecreaseQuantity()
    {
        GetComponentInParent<ShopItem>().DecreaseQuantity();
    }
}