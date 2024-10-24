using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlots : MonoBehaviour
{
    public Sprite Ñhoosen;
    public Sprite No;
    public int Number;
    private Image image;
    public bool activateChest;

    public bool Activate
    {
        get { return activateChest; }
        set
        {
            activateChest = value;
            image.sprite = activateChest ? Ñhoosen : No;

        }
    }
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = No;
    }
    void Update()
    {
        if (Number == ChestCanvas.pickedChestSlot)
        {
            Activate = true;
        }
        else
        {
            Activate = false;
        }
    }
}
