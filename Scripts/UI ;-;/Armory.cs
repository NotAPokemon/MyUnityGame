using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armory : BaseUI
{
    List<Image> images = new List<Image>();

    protected override void openUI()
    {
        for (int i = 0; i < player.items.Count; i++)
        {
            BaseItem item = player.items[i];
            if (item is not NullItem)
            {
                Image image = component.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>();
                image.enabled = true;
                images.Add(image);
                image.sprite = item.icon;
            }
            selected = -1;
        }
    }

    protected override void checkKeys()
    {
        base.checkKeys();
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            toggleOn();
            selected = -1;
            changed = false;
        }
    }

    protected override void ifChanged()
    {
        
    }

    protected override void Update()
    {
        base.Update();
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
