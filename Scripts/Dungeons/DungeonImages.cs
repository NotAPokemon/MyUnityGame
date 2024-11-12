using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonImages : MonoBehaviour
{
    public static Sprite red;
    public static Sprite green;
    public static Sprite blue;
    public static Sprite orange;
    public static Sprite yellow;
    public static Sprite purple;
    public Sprite red_public;
    public Sprite green_public;
    public Sprite blue_public;
    public Sprite orange_public;
    public Sprite yellow_public;
    public Sprite purple_public;

    void Awake()
    {
        red = red_public;
        green = green_public;
        blue = blue_public;
        orange = orange_public;
        yellow = yellow_public;
        purple = purple_public;
    }


}
