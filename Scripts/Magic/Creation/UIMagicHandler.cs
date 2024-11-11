using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMagicHandler : MonoBehaviour
{

    public static Sprite BC;
    public static Sprite AOC;
    public static Sprite AOS;
    public Sprite BC_Object;
    public Sprite AOC_Object;
    public Sprite AOS_Object;
    
    void Start()
    {
        BC = BC_Object;
        AOC = AOC_Object;
        AOS = AOS_Object;
    }
}
