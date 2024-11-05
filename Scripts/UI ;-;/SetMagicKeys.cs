using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;

public class SetMagicKeys : BaseUI
{
    public GameObject errorObject;
    public GameObject[] textInputsObject;
    float timeSinceError = 1;

    TextMeshProUGUI error;
    List<TextMeshProUGUI> textInputs = new List<TextMeshProUGUI>();
    public List<string> paths = new List<string>();

    private void Start()
    {
        error = errorObject.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < textInputsObject.Length; i++)
        {
            textInputs.Add(textInputsObject[i].GetComponent<TextMeshProUGUI>());
        }
    }

    public void checkForValidName()
    {
        bool isValid = true;
        for (int i = 0; i < textInputs.Count; i ++)
        {
            if (!File.Exists("Assets/Magic/" + textInputs[i].text + ".magic"))
            {
                string key = "";
                if (i == 0)
                {
                    key = "F";
                } else if (i == 1)
                {
                    key = "G";
                } else if (i ==2)
                {
                    key = "H";
                } else if (i == 3)
                {
                    key = "J";
                }
                error.SetText("Invalid Spell for key " + key);
                isValid = false;
            } else
            {
                try
                {
                    paths[i] = "Assets/Magic/" + textInputs[i].text + ".magic";
                } catch
                {
                    paths.Add("Assets/Magic/" + textInputs[i].text + ".magic");
                }
                
            }
        }
        if (isValid)
        {
            if (isOpen)
            {
                isOpen = onExit();
            }
            toggleOn();
        }
    }

    protected override bool onExit()
    {
        for (int i = 0; i < textInputs.Count; i++)
        {
            paths[i] = "Assets/Magic/" + textInputs[i].text + ".magic";
        }
        return true;
    }

    protected override void Update()
    {
        base.Update();
        timeSinceError += Time.deltaTime;
        if (timeSinceError > 1)
        {
            error.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            if (isOpen)
            {
                isOpen = onExit();
            }
            toggleOn();
        }
    }

}
