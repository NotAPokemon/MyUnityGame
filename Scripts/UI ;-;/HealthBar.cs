using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        try
        {
            float percent = Player.player.health / Player.player.MaxHealth;
            transform.GetChild(0).localScale = new Vector3(percent, 1, 1);
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(Player.player.health.ToString() + "/" + Player.player.MaxHealth.ToString());
        } catch { }
       
    }
}
