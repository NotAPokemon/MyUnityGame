using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellMaker : BaseUI
{

    string path = null;

    string commands;

    float timeSinceError = 1;

    bool scalingItem;
    bool positioningItem;
    float posZvalue = 0;

    TextMeshProUGUI textInput;

    GameObject lastMadeObject;

    public GameObject imput;
    public GameObject errorObject;

    TextMeshProUGUI error;

    private void Start()
    {
        textInput = imput.GetComponent<TextMeshProUGUI>();
        error = errorObject.GetComponent<TextMeshProUGUI>();
    }

    protected override bool onExit()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            return false;
        }
        Debug.Log(commands);
        File.WriteAllText(path, commands);
        return true;
    }

    public void checkForValidName()
    {
        if (File.Exists("Assets/Magic/" + textInput.text + ".magic"))
        {
            error.text = "Spell Exists";
            error.gameObject.SetActive(true);
            timeSinceError = 0;
        } else
        {
            path = "Assets/Magic/" + textInput.text + ".magic";
            File.Create(path).Close();
            component.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void handleScaling()
    {
        Vector3 cursorPos = Input.mousePosition;

        float distance = Vector3.Distance(transform.position, cursorPos);

        lastMadeObject.transform.localScale = new Vector3(distance, distance, 1);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            scalingItem = false;
            commands += distance + "," + distance + "," + distance + "}];";
        }
    }

    void handlePositioning()
    {
        Vector2 cursorPos = Input.mousePosition;

        lastMadeObject.transform.localPosition = new Vector2(cursorPos.x - 10, cursorPos.y - 10);

        posZvalue += Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            positioningItem = false;
            commands += cursorPos.x + "," + cursorPos.y + "," + posZvalue + "}];";
            posZvalue = 0;
        }

    }


    protected override void openUI()
    {
        path = null;
        lastMadeObject = null;
        Destroy(component.transform.GetChild(1).gameObject);
        GameObject newDisplay = new GameObject("Display");
        newDisplay.transform.SetParent(component.transform);
        newDisplay.transform.localScale = new Vector3(2,1,1);
        newDisplay.transform.localPosition = Vector3.zero;
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
        if (scalingItem)
        {
            handleScaling();
            return;
        } else if (positioningItem)
        {
            Debug.Log("?");
            handlePositioning();
            return;
        }
        if (path != null)
        {
            if (Input.GetKey(KeyCode.B))
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    commands += "[BC{0,0,0}];";
                    GameObject temp = new GameObject("BC");
                    temp.AddComponent<Image>().sprite = UIMagicHandler.BC;
                    temp.transform.SetParent(component.transform.GetChild(1));
                    temp.transform.localPosition = Vector3.zero;
                    lastMadeObject = temp;
                }
            } 
            else if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    commands += "[AOC{0,0,0}];";
                    GameObject temp = new GameObject("AOC");
                    temp.AddComponent<Image>().sprite = UIMagicHandler.AOC;
                    temp.transform.SetParent(component.transform.GetChild(1));
                    temp.transform.localPosition = Vector3.zero;
                    lastMadeObject = temp;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (lastMadeObject != null)
                {
                    commands += "[S{";
                    scalingItem = true;
                }
            } else if (Input.GetKeyDown(KeyCode.P))
            {
                if (lastMadeObject != null)
                {
                    commands += "[P{";
                    positioningItem = true;
                }
            }
        }
    }
}