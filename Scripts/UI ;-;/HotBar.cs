using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{   

    const float xo = 0.0610783f;
    const float yo = 0.6935474f;
    const float zo = 0.6935474f;

    void Update()
    {
        Transform main = transform.GetChild(0);
        for (int i = 0; i < 9; i++)
        {
            Image image = main.GetChild(i).GetChild(0).GetComponent<Image>();
            Transform scaler = main.GetChild(i);
            scaler.localScale = i == Player.player.Num ? new Vector3(xo * 1.5f, yo * 1.5f, zo * 1.5f) : new Vector3(xo,yo,zo);
            image.sprite = Player.player.items[i].icon;
            image.enabled = Player.player.items[i] is not NullItem;
        }
    }
}
