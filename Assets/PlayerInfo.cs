using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public GameObject akai;
    public GameObject dreyar;

    string name;
    public InventorySystem inventory;
    public int health, power, accuracy, communication, XP;

    public enum playerType { Akai = 0, Dreyar = 1 }

    public PlayerInfo(playerType type)
    {
        if (type == playerType.Akai)
        {
            health = 100;
            power = 50;
            accuracy = 70;
            communication = 50;
            XP = 0;
        }
    }
}
