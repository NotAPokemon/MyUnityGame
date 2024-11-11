using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour
{
    public Slider self;
    public TextMeshProUGUI textMeshPro;
    public ColorUpdater colorUpdater;

    public void updateValue()
    {
       textMeshPro.text = (Mathf.Round(self.value * 10)/10).ToString();
       colorUpdater.updateColor();
    }
}
