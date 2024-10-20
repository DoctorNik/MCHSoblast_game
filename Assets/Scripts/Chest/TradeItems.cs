using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeItems : MonoBehaviour
{
    public Item ChestItem;
    public Item InventoryItem;
    public Inventory inventory;
    public ChestCanvas chest_canvas;
    public PlayerHands PlayerHands;
    public Chest _chest;
    public List<Chest> Chests;

    [SerializeField]private bool trading = false;

    public Action<Item, Item> Trade;
    void Start()
    {
        _chest.ThisChest += TradeOn;
        chest_canvas.ChestIsClose += TradeOff;
    }

    void Update()
    {
        if (trading)
        {
            InventoryItem = inventory.inventoryItems[PlayerHands.pickedSlot-1];
            ChestItem = _chest.ChestItems[ChestCanvas.pickedChestSlot - 1];

            if (chest_canvas.CanvasView && Input.GetKeyDown(KeyCode.F))
            {
                Trade?.Invoke(InventoryItem, ChestItem);
            }
        }
    }

    private void TradeOn(Chest c)
    {
        trading = true;
    }
    private void TradeOff()
    {
        trading = false;
    }
}
