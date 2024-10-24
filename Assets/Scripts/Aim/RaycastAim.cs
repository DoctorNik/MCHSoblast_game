using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastAim : MonoBehaviour
{
    public Camera playerCamera;
    public float detectionDistance = 5f;
    public LayerMask layerMask;
    public GameObject canvas;
    public bool looking = false;
    public bool CanPick = false;
    public bool CanOpen = false;
    public Action<RaycastHit> OnAimHit;
    private Aim canvasComponent;
    private ScriptableObject Object;
    public Chest CurrentChest;
    public Container CurrentContainer;
    public event Action<ScriptableObject> OnItemPickedUp;
    public Action<Chest> ChestOpen;
    public Action<Chest> RedrawUpdate;
    public PlayerHands hand;
    public RaycastHit obj;
    public Action<bool> MustStop;

    private void Awake()
    {
        canvasComponent = canvas.GetComponent<Aim>();
        hand = FindObjectOfType<PlayerHands>();
    }
    void Start()
    {
        //canvasComponent = canvas.GetComponent<Aim>();
        OnAimHit += CheckForPickable;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, detectionDistance, layerMask))
        {
            OnAimHit?.Invoke(hit);
        }
        else if (looking) 
        {
            looking = false;
            CanPick = false;
            CanOpen = false;
            CurrentChest = null;
            CurrentContainer = null;
            canvasComponent.ChangeAimFalse();
        }
    }
    private void Update()
    {
        if (CanPick && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Подобран" + Object.name);

            OnItemPickedUp?.Invoke(Object);

            CanPick = false;
        }
        if (CurrentChest != null && !hand.HeavyPicked)
        {
            if (!CurrentChest.IsOpen && CanOpen && Input.GetKeyDown(KeyCode.E))
            {
                RedrawUpdate?.Invoke(CurrentChest);
                CurrentChest.Opening();
                ChestOpen?.Invoke(CurrentChest);
                Debug.Log($"Открытие {CurrentChest}");
                CanOpen = false;
                MustStop?.Invoke(true);
            }
        }
        if (CurrentContainer != null && Input.GetKeyDown(KeyCode.E) && !CurrentContainer.ContainerActivate)
        {
            CurrentContainer.GetObject(hand.handObject);
        }
    }
    public void CanDestroy()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, detectionDistance, layerMask))
        {
            DestroyHitted(hit);
        }
    }
    public void DestroyHitted(RaycastHit hit)
    {
        Destroy(hit.transform.gameObject);
    }
    public void ReceiveItemData(Item itemData)
    {
        Debug.Log("Received item: " + itemData.name);
        Object = itemData;
    }
    public void CheckForPickable(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Pick"))
        {
            CanPick = true;
            if (!looking) 
            {
                ItemHolder itemHolder = hit.transform.GetComponent<ItemHolder>();
                if (canvasComponent != null)
                {
                    canvasComponent.ChangeAimTrue();
                    Debug.Log("Pick рядом");
                    obj = hit;
                }
                itemHolder.GiveItemDataToOtherScript(this);
                looking = true;
            }
        }
        else if (hit.transform.CompareTag("Chest"))
        {
            CanOpen = true;
            CurrentChest = hit.transform.GetComponent<Chest>();
            if (!looking)
            {
                if (canvasComponent != null)
                {
                    canvasComponent.ChangeAimTrue();
                    Debug.Log("Chest рядом");
                }
                looking = true;
            }
        }
        else if (hit.transform.CompareTag("Container"))
        {
            CurrentContainer = hit.transform.GetComponent<Container>();
            if (!looking)
            {
                if (canvasComponent != null)
                {
                    canvasComponent.ChangeAimTrue();
                    Debug.Log("Container рядом");
                }
                looking = true;
            }
        }
    }
    public void RemoveObject()
    {
        if (obj.transform != null) 
        {
            Destroy(obj.transform.gameObject); 
            Debug.Log("Объект удалён!"); 
            obj = new RaycastHit(); 
        }
        else
        {
            Debug.Log("Нет объекта для удаления!"); 
        }
    }
}
