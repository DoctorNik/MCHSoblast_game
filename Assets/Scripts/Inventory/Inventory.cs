using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<Item> onItemAdded;
    [SerializeField]public List<Item> inventoryItems = new List<Item>();

    public void AddItem(Item item)
    {
        inventoryItems.Add(item);

        onItemAdded?.Invoke(item);
    }
}
