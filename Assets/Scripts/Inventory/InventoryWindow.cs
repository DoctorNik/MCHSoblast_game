using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] Inventory target;
    [SerializeField] RectTransform itemspanel;

    List<GameObject> drawnIcons = new List<GameObject>();
    void Start()
    {
        target.onItemAdded += OnItemAdded;
        Redraw();
    }

    void OnItemAdded(Item obj) => Redraw();
    void Redraw()
    {
        ClearDrawn();

        for (var i = 0; i < target.inventoryItems.Count; i++)
        {
            var item = target.inventoryItems[i];
            var icon = new GameObject("Icon");
            var image = icon.AddComponent<Image>();
            image.sprite = item.Icon;

            icon.transform.SetParent(itemspanel, false);

            drawnIcons.Add(icon);
        }
    }

    void ClearDrawn()
    {
        for (var i = 0; i < drawnIcons.Count; i++)
            Destroy(drawnIcons[i]);

        drawnIcons.Clear();
    }
}
