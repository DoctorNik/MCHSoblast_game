using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    public Sprite aim_common;
    public Sprite aim_active;
    public bool aim = false;
    public Image aimImage;

    private void Start()
    {
        UpdateAim();
    }
    public void ChangeAimFalse()
    {
        aim = false;
        UpdateAim();
    }
    public void ChangeAimTrue()
    {
        aim = true;
        UpdateAim();
    }
    private void UpdateAim()
    {
        if (aim)
        {
            aimImage.sprite = aim_active; 
        }
        else
        {
            aimImage.sprite = aim_common; 
        }
    }
}

