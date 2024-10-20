using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<Item> onItemAdded;
    public List<TradeItems> tradeItemsList;
    public RaycastAim raycastAim;
    public PlayerHands hand;
    [SerializeField]public List<Item> inventoryItems = new List<Item>();

    public ScriptableObject Empty;

    public event Action<ScriptableObject> OnItemAddTask;
    public event Action<ScriptableObject> HandObject;

    private void Awake()
    {
        hand = FindObjectOfType<PlayerHands>();
    }
    private void Start()
    {
        raycastAim.OnItemPickedUp += ItemPickedUp;
        tradeItemsList = new List<TradeItems>(FindObjectsOfType<TradeItems>());
        foreach (var tradeItem in tradeItemsList)
        {
            tradeItem.Trade += GetItemToTrade;
        }

    }

    private void ItemPickedUp(ScriptableObject pickedObject)
    {
        Debug.Log("Ща добавим " + pickedObject.name + " на " + hand.pickedSlot + " ячейку");
        AddItem((Item)pickedObject, hand.pickedSlot);
        var item = inventoryItems[hand.pickedSlot - 1];
        hand.itemPrefab = item._prefab;
    }
    private void GetItemToTrade(Item inventory, Item chest)
    {
        Debug.Log($"Меняем {inventory} и {chest}");
        TradeItem(chest, hand.pickedSlot);
    }
    public void AddItem(Item item, int count)
    {
        count -= 1;
        if (count >= 0 && count < inventoryItems.Count)
        {
            if (inventoryItems[count] != Empty)
            {
                Debug.Log("Место занято");
                return;
            }
            else
            {
                inventoryItems[count] = item;
                raycastAim.CanDestroy();
                onItemAdded?.Invoke(item);
                OnItemAddTask?.Invoke(item);
                HandObject?.Invoke(item);
            }
        }
        else
        {
            Debug.Log("Index is out of range.");
        }
    }
    public void TradeItem(Item item, int count)
    {
        count -= 1;
        if (count >= 0 && count < inventoryItems.Count)
        {
            inventoryItems[count] = item;
            onItemAdded?.Invoke(item);
            HandObject?.Invoke(item);
        }
        else
        {
            Debug.Log("Index is out of range.");
        }
    }
}
