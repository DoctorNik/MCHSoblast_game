using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action<Item> onItemAdded;
    public RaycastAim raycastAim;
    public PlayerHands hand;
    [SerializeField]public List<Item> inventoryItems = new List<Item>();

    public ScriptableObject Empty;

    private void Awake()
    {
        hand = FindObjectOfType<PlayerHands>();
    }
    private void Start()
    {
        raycastAim.OnItemPickedUp += ItemPickedUp;
    }

    private void ItemPickedUp(ScriptableObject pickedObject)
    {
        Debug.Log("�� ������� " + pickedObject.name + " �� " + hand.pickedSlot + " ������");
        AddItem((Item)pickedObject, hand.pickedSlot);
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
                onItemAdded?.Invoke(item);
            }
        }
        else
        {
            // ����� �������� ��������� ��������, ���� ������ ������� �� ������� ����������� ���������.
            Debug.LogError("Index is out of range.");
        }
        onItemAdded?.Invoke(item);
    }
}
