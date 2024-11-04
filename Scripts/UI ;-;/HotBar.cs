using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{


    const float x = 0.07367265f;
    const float y = 0.8365569f;
    const float z = 0.8365569f;

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
            scaler.localScale = i == Player.player.Num ? new Vector3(x, y, z) : new Vector3(xo,yo,zo);
            image.sprite = Player.player.items[i].icon;
            image.enabled = Player.player.items[i] is not NullItem;
        }
    }
}
