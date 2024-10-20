using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public ScriptableObject handObject;
    public int pickedSlot;
    private Inventory inventory;

    public GameObject itemPrefab; 
    private GameObject currentItem; 
    public Transform holdPosition; 
    public int PickedSlot
    {
        set {pickedSlot = value;}
        get { return pickedSlot; }
    }
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        inventory.HandObject += GetInfo;
        pickedSlot = 1;
        PickedObject(1, null);
    }
    public void PickedObject(int n, GameObject prefab)
    {
        Item chosenItem = inventory.inventoryItems[n - 1];
        handObject = chosenItem;
        Debug.Log($"Персонаж держит:{handObject}");

        if (currentItem != null)
        {
            Destroy(currentItem);
        }
        if (prefab != null)
        {
            itemPrefab = prefab;
            Hold();
        }
        else
        {
            itemPrefab = null;
            Debug.Log("Префаб не задан.");
        }
    }
    private void GetInfo(ScriptableObject item)
    {
        var obj = inventory.inventoryItems[pickedSlot - 1];
        PickedObject(pickedSlot, obj._prefab);
    }
    public void Hold()
    {
        if (itemPrefab != null)
        {
            currentItem = Instantiate(itemPrefab, holdPosition.position, Quaternion.identity);
            currentItem.transform.parent = holdPosition;
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.Log("Префаб не найден");
        }
    }
}
