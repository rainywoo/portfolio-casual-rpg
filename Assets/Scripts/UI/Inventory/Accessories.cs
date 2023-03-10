using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Accessories,
    Consumables,
    Etc
}
[System.Serializable]
public class Accessories
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;

    public bool Use()
    {
        return false;
    }
}
