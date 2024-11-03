using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    void Update()
    {
        try
        {
            float percent = Player.player.mana / Player.player.MaxMana;
            transform.GetChild(0).localScale = new Vector3(percent, 1, 1);
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(Player.player.mana.ToString() + "/" + Player.player.MaxMana.ToString());
        } catch { }
        
    }
}
