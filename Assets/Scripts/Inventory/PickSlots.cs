using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickSlots : MonoBehaviour
{
    public Sprite Ñhoosen;
    public Sprite No;
    public int Number;
    private Inventory inventory;
    private PlayerHands player;
    private Image image;
    public bool activate;

    public bool Activate
    {
        get { return activate; }
        set
        {
            activate = value;
            image.sprite = activate ? Ñhoosen : No;

        }
    }
    void Start()
    {
        image = GetComponent<Image>(); 
        image.sprite = No;
        player = FindObjectOfType<PlayerHands>();
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Number == player.pickedSlot && player.HeavyPicked)
        {
            inventory.DropItem(player.pickedSlot);
        }
        if (!player.HeavyPicked)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
                {
                    if (Number == i)
                    {
                        Activate = true;

                        if (inventory.inventoryItems.Count >= Number)
                        {
                            TakeItem();
                            /*var item = inventory.inventoryItems[Number - 1];
                            player.pickedSlot = Number;
                            player.PickedObject(Number, item._prefab);*/
                        }
                    }
                    else
                    {
                        Activate = false;
                    }
                }
            }
        }           
    }
    public void TakeItem()
    {
        if (inventory.inventoryItems.Count >= Number)
        {
            var item = inventory.inventoryItems[Number - 1];
            player.pickedSlot = Number;
            player.PickedObject(Number, item._prefab);
        }
    }
}
