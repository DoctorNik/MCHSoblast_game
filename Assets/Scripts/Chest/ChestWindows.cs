using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestWindows : MonoBehaviour
{
    public List<Chest> Chests;
    public RaycastAim raycastAim;
    public ChestCanvas canvas;
    [SerializeField] RectTransform chestspanel;

    List<GameObject> drawnChestIcons = new List<GameObject>();
    private void Awake()
    {
        Chests = new List<Chest>(FindObjectsOfType<Chest>());
        foreach (var chest in Chests)
        {
            chest.ThisChest += Redraw;
            chest.ChestTrade += OnItemAdded;
        }
        //raycastAim.RedrawUpdate += Redraw;
    }
    void Start()
    {
        //canvas.CanvasUpdated += Redraw;
        Chests = new List<Chest>(FindObjectsOfType<Chest>());
        foreach (var chest in Chests)
        {
            chest.ThisChest += Redraw;
            chest.ChestTrade += OnItemAdded;
        }
    }
    void OnItemAdded(Item obj, Chest chest) => Redraw(chest);
    void Redraw(Chest target)
    {
        Debug.Log("Redraw");
        ClearDrawn();

        for (var i = 0; i < target.ChestItems.Count; i++)
        {
            var item = target.ChestItems[i];
            var icon = new GameObject("Icon");
            var image = icon.AddComponent<Image>();
            image.sprite = item.Icon;

            icon.transform.SetParent(chestspanel, false);

            drawnChestIcons.Add(icon);
        }
    }

    void ClearDrawn()
    {
        for (var i = 0; i < drawnChestIcons.Count; i++)
            Destroy(drawnChestIcons[i]);

        drawnChestIcons.Clear();
    }
}
