using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class Chest : MonoBehaviour
{
    public Action<Chest> ThisChest;
    public Action<Item, Chest> ChestTrade;
    public TradeItems _trade;
    [SerializeField] public List<Item> ChestItems = new List<Item>();
    public bool IsOpen;
    public bool WasOpen;
    public ScriptableObject Empty;

    private static int lastId = 0;
    [SerializeField] private int chestId; 
    public int ChestId => chestId; 
    
    private void Awake()
    {
        chestId = lastId++;
        _trade.Trade += GetItemToTrade;
    }
    private void GetItemToTrade(Item inventory, Item chest)
    {
        TradeItems(inventory, ChestCanvas.pickedChestSlot);
    }
    private void TradeItems(Item item, int count)
    {
        count -= 1;
        if (count >= 0 && count < ChestItems.Count)
        {
            ChestItems[count] = item;
            ChestTrade?.Invoke(item, this);
        }
        else
        {
            Debug.Log("Index is out of range.");
        }
    }
    public void Opening()
    {
        ThisChest?.Invoke(this);
        Debug.Log($"ThisChest invoke {this.name}");
        IsOpen = true;
        WasOpen = true;
    }
}
