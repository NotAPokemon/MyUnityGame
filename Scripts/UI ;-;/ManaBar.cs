using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    void Update()
    {
        Transform main = transform.GetChild(0);
        try
        {
            float percent = Player.player.mana / Player.player.MaxMana;
            main.GetChild(0).localScale = new Vector3(percent, 1, 1);
            main.GetChild(1).GetComponent<TextMeshProUGUI>().SetText( ((int) Player.player.mana).ToString() + "/" + Player.player.MaxMana.ToString());
        } catch { }
        main.gameObject.SetActive(!UIManager.UIOpen);
    }
}
