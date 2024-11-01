using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseUI
{
    List<Image> images = new List<Image>();
    public int LastClicked = -1;
    public NullItem nullItemthis;
    public static NullItem nullItem;
    public GameObject fillerss;
    public static GameObject fillers;


    private void Awake()
    {
        nullItem = nullItemthis;
        fillers = fillerss;
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
        if (LastClicked != -1)
        {
            changed = false;
            Image LastImage = component.transform.GetChild(LastClicked).GetChild(0).GetComponentInChildren<Image>();
            Sprite lastSprite;
            Image Clicked = component.transform.GetChild(selected).GetChild(0).GetComponentInChildren<Image>();
            Sprite clickedSprite;
            try
            {
                player.items[LastClicked].GetType();
                lastSprite = LastImage.sprite;
                if (player.items[LastClicked] is NullItem)
                {
                    lastSprite = null;
                }
            }
            catch
            {
                lastSprite = null;
            }
            try
            {
                player.items[selected].GetType();
                clickedSprite = Clicked.sprite;
                if (player.items[selected] is NullItem)
                {
                    clickedSprite = null;
                }
            }
            catch
            {
                clickedSprite = null;
            }

            LastImage.sprite = clickedSprite;
            Clicked.sprite = lastSprite;
            Clicked.enabled = lastSprite != null;
            LastImage.enabled = clickedSprite != null;

            player.swapItems(selected, LastClicked);
            selected = -1;
        }
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
