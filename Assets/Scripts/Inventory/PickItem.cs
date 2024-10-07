using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] Inventory target;
    RaycastAim _aim;
    public Item itemToAdd;

    private void Awake()
    {
        _aim = GetComponent<RaycastAim>();
    }
    void Start()
    {
        itemToAdd = null;
    }

    void Update()
    {

    }
}
