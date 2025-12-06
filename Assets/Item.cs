using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Item
{
    public enum ItemType
    {
        APPLE = 0,
        MEAT = 1,
        GOLD = 2,
        RED_DIAMOND = 3,
        BLUE_DIAMOND = 4,
        YELLOW_DIAMOND = 5,
        SWORD = 6,
        BATON = 7
    }

    public enum ItemFamilyType
    {
        FOOD = 0,
        LOOT = 1,
        WEAPON = 3
    }

    public string name, description;
    public int price, healthBenefits, damage, nb, maxNb;
    public ItemType type;
    public string article;
    public ItemFamilyType familyType;

    public Item(ItemType type)
    {
        switch (type)
        {
            case ItemType.APPLE:
                name = "Apple";
                price = 50;
                healthBenefits = 10;
                damage = 0;
                nb = 1;
                maxNb = 5;
                description = "A juicy apple.";
                familyType = ItemFamilyType.FOOD;
                article = "an";
                break;

            case ItemType.MEAT:
                name = "Meat";
                price = 50;
                healthBenefits = 30;
                damage = 0;
                nb = 1;
                maxNb = 2;
                description = "A nice piece of cooked meat, to nurture your muscles.";
                familyType = ItemFamilyType.FOOD;
                article = "some";
                break;

            case ItemType.RED_DIAMOND:
                name = "Red diamond";
                price = 250;
                healthBenefits = 0;
                damage = 0;
                nb = 1;
                maxNb = 10;
                description = "A valuable diamond crafted by the best jewellers with some known magic properties.";
                familyType = ItemFamilyType.LOOT;
                article = "a";
                break;

            case ItemType.YELLOW_DIAMOND:
                name = "Yellow diamond";
                price = 200;
                healthBenefits = 0;
                damage = 0;
                nb = 1;
                maxNb = 10;
                description = "A valuable diamond crafted by the best jewellers with some known magic properties.";
                familyType = ItemFamilyType.LOOT;
                article = "a";
                break;

            case ItemType.BLUE_DIAMOND:
                name = "Blue diamond";
                price = 100;
                healthBenefits = 0;
                damage = 0;
                nb = 1;
                maxNb = 10;
                description = "A valuable diamond crafted by the best jewellers with some known magic properties.";
                familyType = ItemFamilyType.LOOT;
                article = "a";
                break;

            case ItemType.GOLD:
                name = "Gold";
                price = 100;
                healthBenefits = 0;
                damage = 0;
                nb = 1;
                maxNb = 1000;
                description = "Gold Coins";
                familyType = ItemFamilyType.LOOT;
                article = "some";
                break;

            case ItemType.SWORD:
                name = "Sword";
                price = 100;
                healthBenefits = 0;
                damage = 10;
                nb = 1;
                maxNb = 1;
                description = "A powerful sword that defeats most opponents.";
                familyType = ItemFamilyType.WEAPON;
                article = "a";
                break;

            case ItemType.BATON:
                name = "Baton";
                price = 50;
                healthBenefits = 0;
                damage = 50;
                nb = 1;
                maxNb = 1;
                description = "A simple wooden stick that you can handle easily.";
                familyType = ItemFamilyType.WEAPON;
                article = "a";
                break;

            default:
                break;
        }

        this.type = type;
    }

    public Texture GetTexture()
    {
        string path = this.name.Replace(" ", "_");

        if (this.familyType == ItemFamilyType.WEAPON)
            return Resources.Load<Texture2D>("weapons/" + path);
        else if (this.familyType == ItemFamilyType.FOOD)
            return Resources.Load<Texture2D>("food/" + path);
        else if (this.familyType == ItemFamilyType.LOOT)
            return Resources.Load<Texture2D>("loot/" + path);
        else
            return null;
    }


    public string ItemInfo()
    {
        string info = "name:" + this.name + ", health benefits:" + this.healthBenefits + ", dammage" + this.damage + ", nb:" + this.nb + ", max Nb" + this.maxNb + ", type" + this.type; return info;
    }

 }
