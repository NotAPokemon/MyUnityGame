using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    public Player player;
    protected bool isOpen = false;
    public GameObject component;
    public KeyCode keyCode;
    

    protected virtual void openUI()
    {

    }

    public void toggleOn()
    {
        isOpen = !isOpen;
    }

    protected virtual void checkKeys()
    {
        if (Input.GetKeyDown(keyCode))
        {
            openUI();
            toggleOn();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            isOpen = !isOpen;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        component.SetActive(isOpen);
        checkKeys();
    }
}
