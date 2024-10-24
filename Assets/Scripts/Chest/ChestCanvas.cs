using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCanvas : MonoBehaviour
{
    public bool CanvasView;
    public RaycastAim raycastAim;
    public GameObject chestPanel;

    public static int pickedChestSlot = 2;
    public int SlotChes;

    public Chest OpenedChest;

    public Action<Chest> ChestIsOpen;
    public Action ChestIsClose;

    private List<Chest> allChests;
    public Action<Chest> CanvasUpdated;
    void Start()
    {
        allChests = new List<Chest>(FindObjectsOfType<Chest>());
        foreach (var chest in allChests)
        {
            chest.ThisChest += HandleChestEvent;
        }

        SlotChes = pickedChestSlot;
        raycastAim.ChestOpen += OpenChest;
        CanvasView = false;
        UpdateCanvasVisibility();
    }
    void HandleChestEvent(Chest chest)
    {
        Debug.Log("HandleChestEvent вызвано для сундука: " + chest.name);
    }
    void OpenChest(Chest chest)
    {
        CanvasView = true;
        OpenedChest = chest;
        UpdateCanvasVisibility();
        ChestIsOpen?.Invoke(chest);
    }
    void UpdateCanvasVisibility()
    {
        if (chestPanel != null) 
        {
            chestPanel.SetActive(CanvasView);
        }
        CanvasUpdated?.Invoke(OpenedChest);
    }
    void Update()
    {
        if (CanvasView)
        {
            SlotChes = pickedChestSlot;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (pickedChestSlot > 1)
                {
                    pickedChestSlot--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (pickedChestSlot < 3)
                {
                    pickedChestSlot++;
                }
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                OpenedChest.IsOpen = false;
                CanvasView = false;
                ChestIsClose?.Invoke();
                raycastAim.CanOpen = false;
                UpdateCanvasVisibility();
                raycastAim.MustStop(false);
            }
        }
    }
}
