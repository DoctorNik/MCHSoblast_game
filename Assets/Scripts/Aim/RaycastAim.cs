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
    private bool looking = false;
    public Action<RaycastHit> OnAimHit;
    private Aim canvasComponent;
    void Start()
    {
        canvasComponent = canvas.GetComponent<Aim>();
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
            canvasComponent.ChangeAimFalse();
        }
    }
    private void Update()
    {

    }
    public void ReceiveItemData(Item itemData)
    {
        Debug.Log("Received item: " + itemData.name);
    }
    public void CheckForPickable(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Pick"))
        {
            if (!looking) 
            {
                ItemHolder itemHolder = hit.transform.GetComponent<ItemHolder>();
                if (canvasComponent != null)
                {
                    canvasComponent.ChangeAimTrue();
                    Debug.Log("Pick рядом");
                }
                itemHolder.GiveItemDataToOtherScript(this);
                looking = true;
            }
        }
    }
}
