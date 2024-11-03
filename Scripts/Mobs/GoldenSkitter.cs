using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenSkitter : Skitter
{
    public GameObject title;
    float timer;

    protected override void Start()
    {
        base.Start();
        title.SetActive(true);
        timer = 0;
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer > 0.5)
        {
            title.SetActive(false);
        }
    }
}
