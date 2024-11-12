using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public float density;
    public float manaAmount;

    void Start()
    {
        Sprite color;

        if (density >= 0.9)
        {
            color = DungeonImages.purple;
        }
        else if (density >= 0.75)
        {
            color = DungeonImages.red;
        }
        else if (density >= 0.6)
        {
            color = DungeonImages.orange;
        }
        else if (density >= 0.4)
        {
            color = DungeonImages.yellow;
        }
        else if (density >= 0.25)
        {
            color = DungeonImages.green;
        } else
        {
            color = DungeonImages.blue;
        }


        SpriteRenderer image = transform.GetComponent<SpriteRenderer>();
        if (image != null)
        {
            image.sprite = color;
        } else
        {
            image = transform.AddComponent<SpriteRenderer>();
            image.sprite = color;
        }
    }

    void handleEnter()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, manaAmount + transform.localRotation.eulerAngles.z);
    }
}
