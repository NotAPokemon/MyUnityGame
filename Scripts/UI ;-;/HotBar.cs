using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    void Update()
    {
        Transform main = transform.GetChild(0);
        for (int i = 0; i < 9; i++)
        {
            Image image = main.GetChild(i).GetChild(0).GetComponent<Image>();
            image.sprite = Player.player.items[i].icon;
            image.enabled = Player.player.items[i] is not NullItem;
        }
    }
}
