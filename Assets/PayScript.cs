using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LeaveShop() 
    {
        List<Item> purchasedItems = new List<Item>(); 
        purchasedItems = GameObject.Find("shopSystem").GetComponent<ShopSystem>().shopItems; 
        int moneyleft = GameObject.Find("shopSystem").GetComponent<ShopSystem>().moneyLeft; 
        GameObject.Find("Player").GetComponent<InventorySystem>().SetMoney(moneyleft);
        GameObject.Find("Player").GetComponent<InventorySystem>().AddPurchasedItems(purchasedItems); GameObject.Find("Player").GetComponent<ControlPlayer>().shopIsDisplayed = false; GameObject.Find("Player").transform.position -= GameObject.Find("Player").transform.forward * 4; GameObject.Find("Player").transform.Rotate(0, 180, 0); GameObject.Find("shopUI").SetActive(false);

    }
}
