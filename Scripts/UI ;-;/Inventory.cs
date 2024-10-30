using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BaseUI
{
    List<Image> images = new List<Image>();
    public int LastClicked = -1;
    public int Selected = 0;
    public bool changed = false;
    public NullItem nullItem;
    public GameObject fillers;
    GameObject main;


    void Start()
    {
        main = transform.GetChild(0).gameObject;
    }

    protected override void openUI()
    {
        for (int i = 0; i < player.items.Count; i++)
        {
            BaseItem item = player.items[i];
            if (item is not NullItem)
            {
                Image image = main.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>();
                image.enabled = true;
                images.Add(image);
                image.sprite = item.icon;
            }
        }
    }

    public void swapItems(int a, int b)
    {
        if (a < 0 || b < 0)
        {
            Debug.LogError("Indices must be non-negative.");
            return;
        }

        int max = Mathf.Max(a, b);
        while (player.items.Count <= max)
        {
            NullItem filler = Instantiate(nullItem);
            filler.transform.SetParent(fillers.transform);
            player.items.Add(filler);
        }

        // Swap items
        BaseItem temp = player.items[a];
        player.items[a] = player.items[b];
        player.items[b] = temp;
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
        if (changed)
        {
            if (LastClicked != -1)
            {
                changed = false;
                Image LastImage = main.transform.GetChild(LastClicked).GetChild(0).GetComponentInChildren<Image>();
                Sprite lastSprite;
                Image Clicked = main.transform.GetChild(Selected).GetChild(0).GetComponentInChildren<Image>();
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
                    player.items[Selected].GetType();
                    clickedSprite = Clicked.sprite;
                    if (player.items[Selected] is NullItem)
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
                
                swapItems(Selected, LastClicked);
                LastClicked = -1;
            }
        }
    }

}
