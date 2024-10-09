using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public ScriptableObject handObject;
    public int pickedSlot;
    private Inventory inventory;
    public int PickedSlot
    {
        set {pickedSlot = value;}
        get { return pickedSlot; }
    }
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        pickedSlot = 1;
        PickedObject(1);
    }
    public void PickedObject(int n)
    {
        Item chosenItem = inventory.inventoryItems[n - 1];
        handObject = chosenItem;
        Debug.Log($"Персонаж держит:{handObject}");
    }
}
