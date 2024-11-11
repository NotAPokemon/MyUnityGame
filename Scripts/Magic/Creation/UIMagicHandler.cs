using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMagicHandler : MonoBehaviour
{

    public static Sprite BC;
    public static Sprite AOC;
    public static Sprite AOS;
    public static Sprite AOB;
    public Sprite BC_Object;
    public Sprite AOC_Object;
    public Sprite AOS_Object;
    public Sprite AOB_Object;
    
    void Start()
    {
        BC = BC_Object;
        AOC = AOC_Object;
        AOS = AOS_Object;
        AOB = AOB_Object;
    }
}
