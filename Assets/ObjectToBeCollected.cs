using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToBeCollected : MonoBehaviour
{
    public GameObject baton, sword, apple, meat, gold, redDiamond, yellowDiamond, blueDiamond;
    public Item.ItemType type;
    public Item item;

    void Start()
    {
        item = new Item(type);
        GameObject g = new GameObject();

        switch (type)
        {
            case Item.ItemType.BATON:
                g = baton;
                break;
            case Item.ItemType.SWORD:
                g = sword;
                break;
            case Item.ItemType.APPLE:
                g = apple;
                break;
            case Item.ItemType.MEAT:
                g = meat;
                break;
            case Item.ItemType.GOLD:
                g = gold;
                break;
            case Item.ItemType.RED_DIAMOND:
                g = redDiamond;
                break;
            case Item.ItemType.YELLOW_DIAMOND:
                g = yellowDiamond;
                break;
            case Item.ItemType.BLUE_DIAMOND:
                g = blueDiamond;
                break;
            default:
                break;
        }

        GameObject g1 = Instantiate(g, transform.position, Quaternion.identity);
        g1.transform.parent = transform;
    }

    void Update()
    {
    }
}
