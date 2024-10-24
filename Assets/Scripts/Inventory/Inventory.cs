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
    public event Action ItemDeleted;
    public event Action<ScriptableObject> HandObject;

    public List<Container> Containers;
    private void Awake()
    {
        raycastAim.OnItemPickedUp += ItemPickedUp;
        hand = FindObjectOfType<PlayerHands>();
        Containers = new List<Container>(FindObjectsOfType<Container>());
        foreach (var cont in Containers)
        {
            cont.ContainerActivated += DeleteObject;
        }
    }
    private void Start()
    {
        tradeItemsList = new List<TradeItems>(FindObjectsOfType<TradeItems>());
        foreach (var tradeItem in tradeItemsList)
        {
            tradeItem.Trade += GetItemToTrade;
        }

    }

    private void ItemPickedUp(ScriptableObject pickedObject)
    {
        //Debug.Log("�� ������� " + pickedObject.name + " �� " + hand.pickedSlot + " ������");
        AddItem((Item)pickedObject, hand.pickedSlot);
        var item = inventoryItems[hand.pickedSlot - 1];
        hand.itemPrefab = item._prefab;
    }
    private void GetItemToTrade(Item inventory, Item chest)
    {
        Debug.Log($"������ {inventory} � {chest}");
        TradeItem(chest, hand.pickedSlot);
    }
    public void AddItem(Item item, int count)
    {
        count -= 1;
        if (count >= 0 && count < inventoryItems.Count)
        {
            if (inventoryItems[count] != Empty)
            {
                Debug.Log("����� ������");
                return;
            }
            else
            {
                inventoryItems[count] = item;
                raycastAim.CanDestroy();
                raycastAim.RemoveObject();
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
    /*public void DropItem(int count)
    {
        count -= 1;
        if (inventoryItems[count] != Empty) 
        {
            inventoryItems[count] = (Item)Empty; 
        }
        else
        {
            Debug.Log("������ �����, ������ �����������.");
        }
        ItemDeleted?.Invoke();
        hand.PickedObject(count + 1, null);
    }*/

    public void DropItem(int slot)
    {
        slot -= 1;

        if (slot >= 0 && slot < inventoryItems.Count && inventoryItems[slot] != Empty)
        {
            Transform playerTransform = hand.transform;
            GameObject itemPrefab = inventoryItems[slot]._prefab;

            if (itemPrefab != null)
            {
                Vector3 dropPosition = playerTransform.position + Vector3.down * 1f;
                Instantiate(itemPrefab, dropPosition, Quaternion.identity);

                inventoryItems[slot] = (Item)Empty;
            }
            ItemDeleted?.Invoke();
            hand.PickedObject(slot + 1, null);
        }
        else
        {
            if (slot >= 0 && slot < inventoryItems.Count && inventoryItems[slot] == Empty)
            {
                Debug.Log("������ �����, ������ �����������.");
            }
            else
            {
                Debug.Log("������������ ������ ��� � ������ ��� ���������.");
            }
        }
    }

    private void DeleteObject(Container cont, bool MustDelete)
    {
        if (MustDelete)
        {
            inventoryItems[hand.pickedSlot - 1] = (Item)Empty;
            ItemDeleted?.Invoke();
            hand.PickedObject(hand.pickedSlot, null);
        }
    }
}
