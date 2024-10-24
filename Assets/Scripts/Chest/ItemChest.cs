using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemChest : MonoBehaviour, IPointerClickHandler
{
    private Item item;
    private Chest chest;
    public int Number;

    public void SetItem(Item newItem, Chest newChest, int number)
    {
        item = newItem;
        chest = newChest;
        Number = number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Предмет {item.name} в ячейке {Number} был нажат");
        ChestCanvas.pickedChestSlot = Number;
    }
}
