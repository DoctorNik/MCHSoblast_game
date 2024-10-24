using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]public ScriptableObject ObjectNeed;
    public Action<Container, bool> ContainerActivated;
    public bool ContainerActivate;
    public bool MustDelete;

    public void GetObject(ScriptableObject obj)
    {
        if (obj == ObjectNeed)
        {
            Debug.Log("Container активирован");
            ContainerActivated?.Invoke(this, MustDelete);
            ContainerActivate = true;
        }
        else
        {
            Debug.Log("Данный объект не нужен");
        }
    }
}
