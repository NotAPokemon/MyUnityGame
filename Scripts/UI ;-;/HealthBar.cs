using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

        Transform main = transform.GetChild(0);

        try
        {
            float percent = Player.player.health / Player.player.MaxHealth;
            main.GetChild(0).localScale = new Vector3(percent, 1, 1);
            main.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(((int)Player.player.health).ToString() + "/" + Player.player.MaxHealth.ToString());
            
        } catch { }

        main.gameObject.SetActive(!UIManager.UIOpen);

    }
}
