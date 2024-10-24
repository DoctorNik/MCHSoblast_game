using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public Item handObject;
    public int pickedSlot;
    private Inventory inventory;

    public GameObject itemPrefab; 
    private GameObject currentItem; 
    public Transform holdPosition;

    public bool HeavyPicked = false;
    public int PickedSlot
    {
        set {pickedSlot = value;}
        get { return pickedSlot; }
    }
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        inventory.HandObject += GetInfo;
        inventory.ItemDeleted += CheckHeavy;
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
        if (itemPrefab != null && !handObject.Heavy)
        {
            currentItem = Instantiate(itemPrefab, holdPosition.position, Quaternion.identity);
            currentItem.transform.parent = holdPosition;

            HoldingCoordinates holdingCoordinates = currentItem.GetComponent<HoldingCoordinates>();
            if (holdingCoordinates != null)
            {
                currentItem.transform.localPosition = holdingCoordinates.positionOffset;
                currentItem.transform.localRotation = Quaternion.Euler(holdingCoordinates.rotationOffset);
            }
            else
            {
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
            }
        }
        else if (itemPrefab != null && handObject.Heavy)
        {
            currentItem = Instantiate(itemPrefab, holdPosition.position, Quaternion.identity);
            currentItem.transform.parent = holdPosition;

            HoldingCoordinates holdingCoordinates = currentItem.GetComponent<HoldingCoordinates>();
            if (holdingCoordinates != null)
            {
                currentItem.transform.localPosition = holdingCoordinates.positionOffset;
                currentItem.transform.localRotation = Quaternion.Euler(holdingCoordinates.rotationOffset);
            }
            else
            {
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
            }
            HeavyPicked = true;
        }
    }
    public void CheckHeavy()
    {
        if (HeavyPicked)
        {
            HeavyPicked = false;
        }
    }
}
