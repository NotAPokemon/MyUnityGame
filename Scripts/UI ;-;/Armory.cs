using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Armory : BaseUI
{
    List<Image> images = new List<Image>();
    public static Armory armory;

    void Awake()
    {
        armory = this;
    }

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
        for (int i = 0;i < player.armors.Count; i++)
        {
            BaseItem armor = player.armors[i];
            if (armor is not NullItem)
            {
                Image image = component.transform.GetChild(i + 30).GetChild(0).GetComponentInChildren<Image>();
                image.enabled = true;
                images.Add(image);
                image.sprite = armor.icon;
            }
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

    void updateSlot(BaseItem item, int index)
    {
        
        Image image = component.transform.GetChild(index).GetChild(0).GetComponentInChildren<Image>();
        if (item is null)
        {
            image.sprite = null;
            image.enabled = false;
        } else
        {
            image.sprite = item.icon;
            image.enabled = item is not NullItem;
        }
    }

    protected override void ifChanged()
    {
        changed = false;
        if (selected != -1)
        {
            if (selected >= 30 && player.armors[selected - 30] is not NullItem)
            {
                player.addItem(player.armors[selected - 30]);
                for (int i = 0; i < player.items.Count; i++)
                {
                    if (player.items[i].Equals(player.armors[selected - 30]))
                    {
                        updateSlot(null, selected);
                        updateSlot(player.armors[selected - 30], i);
                    }

                }
                player.armors[selected - 30] = Player.makeNullItem();
                
            }
            else if (player.items[selected] is not NullItem && player.items[selected] is BaseArmor)
            {
                int armorIndex = (int)(player.items[selected] as BaseArmor).type;
                Player.player.swarpArmor(armorIndex, selected);
                updateSlot(player.armors[armorIndex], armorIndex + 30);
                updateSlot(player.items[selected], selected);

            }
        }
        selected = -1;
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
