using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicHandler : MonoBehaviour
{

    public static Sprite BC;
    public static Sprite AOC;
    public Sprite BC_Object;
    public Sprite AOC_Object;
    
    void Start()
    {
        BC = BC_Object;
        AOC = AOC_Object;
    }
}
