using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class RaycastAim : MonoBehaviour
{
    public Camera playerCamera;
    public float detectionDistance = 5f;
    public LayerMask layerMask;
    public GameObject canvas;
    private bool looking = false;
    private bool CanPick = false;
    public Action<RaycastHit> OnAimHit;
    private Aim canvasComponent;
    private ScriptableObject Object;

    public event Action<ScriptableObject> OnItemPickedUp;
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
            CanPick = false;
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
                }
                itemHolder.GiveItemDataToOtherScript(this);
                looking = true;
            }
        }
    }
}
