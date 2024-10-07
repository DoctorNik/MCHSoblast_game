using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public Item itemData;

    public void GiveItemDataToOtherScript(RaycastAim target)
    {
        target.ReceiveItemData(itemData);
    }
}
