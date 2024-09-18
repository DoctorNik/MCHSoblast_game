using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] Inventory target;

    [SerializeField] private Item Sphere; 
    [SerializeField] private Item Cube;
    [SerializeField] private Item Cylinder;

    Item itemToAdd;
    void Start()
    {
        itemToAdd = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemToAdd = Sphere; 
            target.AddItem(itemToAdd); 
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemToAdd = Cube; 
            target.AddItem(itemToAdd); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            itemToAdd = Cylinder;
            target.AddItem(itemToAdd);
        }
    }
}
