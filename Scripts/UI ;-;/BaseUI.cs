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
    public bool changed = false;
    public int selected = -1;


    protected virtual void openUI()
    {

    }

    public void toggleOn()
    {
        isOpen = !isOpen;
        UIManager.UIOpen = isOpen;
        UIManager.ui = isOpen ? this : null;
    }

    protected virtual void checkKeys()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (!UIManager.UIOpen || UIManager.ui.Equals(this)) 
            {
                openUI();
                toggleOn();
            }
        }
    }

    protected virtual void ifChanged()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        component.SetActive(isOpen);
        checkKeys();
        player.locked = UIManager.UIOpen;
        player.mouse.Locked = !UIManager.UIOpen;
        if (changed)
        {
            ifChanged();
        }
    }
}
