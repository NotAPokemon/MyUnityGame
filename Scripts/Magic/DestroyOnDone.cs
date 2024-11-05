using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDone : MonoBehaviour
{

    float amount;
    bool running = false;

    public void sustain(float amount)
    {
        running = true;
        this.amount = amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (Player.player.mana >= amount)
            {
                Player.player.mana -= amount * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
