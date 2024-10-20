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
            if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) ||
                Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.F) &&
                    !Input.GetKey(KeyCode.Alpha1) && !Input.GetKey(KeyCode.Alpha2) &&
                    !Input.GetKey(KeyCode.Alpha3) && !Input.GetKey(KeyCode.Alpha4) &&
                    !Input.GetKey(KeyCode.Alpha5))
                {
                    OpenedChest.IsOpen = false;
                    CanvasView = false;
                    ChestIsClose?.Invoke();
                    raycastAim.CanOpen = false;
                    UpdateCanvasVisibility();
                }
            }
        }
    }
}
