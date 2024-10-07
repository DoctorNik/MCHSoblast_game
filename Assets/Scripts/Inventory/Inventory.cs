using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<Item> onItemAdded;
    [SerializeField]public List<Item> inventoryItems = new List<Item>();

    public void AddItem(Item item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i < inventoryItems.Count)
            {
                inventoryItems[i] = item;
            }
            else
            {
                break;
            }
        }
        onItemAdded?.Invoke(item);
    }
}
