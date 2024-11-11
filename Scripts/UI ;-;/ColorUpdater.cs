using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorUpdater : MonoBehaviour
{
    public Slider r;
    public Slider g;
    public Slider b;
    public Slider a;
    public Image self;

    public void updateColor()
    {
        self.color = new Color(r.value / 255,g.value / 255,b.value/ 255,a.value);
    }

}
