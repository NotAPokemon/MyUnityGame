using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseUI
{
    protected override void openUI()
    {
        for (int i = 0; i < player.items.Count; i++)
        {
            BaseItem item = player.items[i];
            string slotName = "Slot 1 (" + (i-1) + ")";
            GameObject slot = GameObject.Find(slotName);
            Image image = slot.GetComponentInChildren<Image>();
            image.transform.gameObject.SetActive(true);
            image.sprite = CreateSpriteFromGameObject(item.gameObject);
        }
    }


    protected override void Update()
    {
        base.Update();
        player.locked = isOpen;
        player.mouse.Locked = !isOpen;
    }

}
