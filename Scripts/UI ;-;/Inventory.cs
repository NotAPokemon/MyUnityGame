using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseUI
{
    List<Image> images = new List<Image>();

    protected override void openUI()
    {
        for (int i = 0; i < player.items.Count; i++)
        {
            BaseItem item = player.items[i];
            GameObject slot = transform.GetChild(0).gameObject;
            Image image = slot.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>();
            image.enabled = true;
            images.Add(image);
            image.sprite = item.icon;
        }
    }


    protected override void Update()
    {
        base.Update();
        player.locked = isOpen;
        player.mouse.Locked = !isOpen;
        if (!isOpen && images.Count >= 1)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].enabled = false;
            }
            images.Clear();
        }
    }

}
