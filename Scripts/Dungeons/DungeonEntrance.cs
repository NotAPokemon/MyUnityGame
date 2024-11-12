using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public float density;
    public float manaAmount;

    string nameColor;

    void Start()
    {
        Sprite color;

        if (density >= 0.9)
        {
            color = DungeonImages.purple;
            nameColor = "S";
        }
        else if (density >= 0.75)
        {
            color = DungeonImages.red;
            nameColor = "A";
        }
        else if (density >= 0.6)
        {
            color = DungeonImages.orange;
            nameColor = "B";
        }
        else if (density >= 0.4)
        {
            color = DungeonImages.yellow;
            nameColor = "C";
        }
        else if (density >= 0.25)
        {
            color = DungeonImages.green;
            nameColor = "D";
        } else
        {
            color = DungeonImages.blue;
            nameColor = "E";
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
        Debug.Log(nameColor);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, manaAmount + transform.localRotation.eulerAngles.z);
        if (Vector3.Distance(transform.position, Player.player.transform.position) <= 2)
        {
            handleEnter();
        }
    }
}
