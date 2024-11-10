using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class SpellMaker : BaseUI
{

    string path = null;

    string commands;

    float timeSinceError = 1;

    bool scalingItem;
    bool positioningItem;
    bool rotatingItem;
    bool materialSelecting;
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
        if (commands == null)
        {
            return true;
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

        posZvalue += Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            scalingItem = false;
            if (posZvalue == 0)
            {
                posZvalue = distance;
            }
            commands += distance + "," + distance + "," + posZvalue + "}];";
            posZvalue = 0;
        }
    }

    void handlePositioning()
    {
        Vector2 cursorPos = Input.mousePosition;

        lastMadeObject.transform.position = new Vector2(cursorPos.x, cursorPos.y);

        posZvalue += Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            positioningItem = false;
            commands += cursorPos.x + "," + cursorPos.y + "," + posZvalue + "}];";
            posZvalue = 0;
        }

    }

    void handleRotation()
    {
        float MouseHorizontal = Input.GetAxis("Mouse X");
        float MouseVertical = Input.GetAxis("Mouse Y");

        lastMadeObject.transform.localRotation = Quaternion.Euler(lastMadeObject.transform.localRotation.x + MouseVertical, lastMadeObject.transform.localRotation.y + MouseHorizontal, 0);

        posZvalue += Input.GetAxis("Mouse ScrollWheel");

        if ( Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            rotatingItem = false;
            commands += lastMadeObject.transform.localRotation.x + "," + lastMadeObject.transform.localRotation.y + "," + posZvalue + "}];";
            posZvalue = 0;
        }
    }

    void handleMaterialSelection()
    {
        
    }


    protected override void openUI()
    {
        path = null;
        lastMadeObject = null;
        Destroy(component.transform.GetChild(1).gameObject);
        GameObject newDisplay = new GameObject("Display");
        newDisplay.transform.SetParent(component.transform);
        newDisplay.transform.localScale = new Vector3(2, 1, 1);
        newDisplay.transform.localPosition = Vector3.zero;
    }

    void HandleKeyBinds()
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
        else if (Input.GetKeyDown(KeyCode.S) && lastMadeObject != null)
        {

            commands += "[S{";
            scalingItem = true;

        }
        else if (Input.GetKeyDown(KeyCode.P) && lastMadeObject != null)
        {
            commands += "[P{";
            positioningItem = true;

        }
        else if (Input.GetKeyDown(KeyCode.R) && lastMadeObject != null)
        {
            commands += "[R{";
            rotatingItem = true;
        } else if (Input.GetKeyDown(KeyCode.M) && lastMadeObject != null)
        {
            commands += "[M{";
            materialSelecting = true;
        }
    }


    bool earlyReturn()
    {
        if (scalingItem)
        {
            handleScaling();
            return true;
        }
        else if (positioningItem)
        {
            handlePositioning();
            return true;
        }
        else if (rotatingItem)
        {
            handleRotation();
            return true;
        } else if (materialSelecting)
        {
            handleMaterialSelection(); 
            return true;
        }

        return false;
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
            isOpen = onExit();
            toggleOn();
        }

        if (earlyReturn())
        {
            return;
        }
        
        if (path != null)
        {
            HandleKeyBinds();
        }
    }
}
