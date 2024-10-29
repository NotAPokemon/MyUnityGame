using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKeys : MonoBehaviour
{
    public Player player;
    public Skitter Skitter;
    GameObject e;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            e = Instantiate(Skitter.transform.gameObject);
            e.transform.position = player.transform.position;
            e.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            e.GetComponent<Skitter>().health = 0;
        }
    }
}
