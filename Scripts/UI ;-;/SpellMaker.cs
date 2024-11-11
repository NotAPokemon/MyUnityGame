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
    static bool materialArived = false;
    float posZvalue = 0;

    TextMeshProUGUI textInput;

    GameObject lastMadeObject;

    public GameObject imput;
    public GameObject errorObject;

    TextMeshProUGUI error;

    public GameObject materialMakerPrefab;

    GameObject materialMaker;

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
        if (lastMadeObject.name == "BC")
        {
            lastMadeObject.transform.localScale = new Vector3(distance * 2, distance, 1);
        }

        posZvalue += Input.GetAxis("Mouse ScrollWheel") * 100;


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

        posZvalue += Input.GetAxis("Mouse ScrollWheel") * 100;


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            positioningItem = false;
            commands += cursorPos.x / 50 + "," + cursorPos.y/ 100 + "," + posZvalue + "}];";
            posZvalue = 0;
        }

    }

    void handleRotation()
    {
        float MouseHorizontal = Input.mousePosition.x;
        float MouseVertical = Input.mousePosition.y;

        posZvalue += Input.GetAxis("Mouse ScrollWheel") * 50;

        lastMadeObject.transform.localRotation = Quaternion.Euler(lastMadeObject.transform.localRotation.x + MouseVertical, lastMadeObject.transform.localRotation.y + MouseHorizontal, posZvalue);

        if ( Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            rotatingItem = false;
            commands += lastMadeObject.transform.localRotation.x + "," + lastMadeObject.transform.localRotation.y + "," + posZvalue + "}];";
            posZvalue = 0;
        }
    }


    public static void dropMaterial()
    {
        materialArived = true;
    }

    void handleMaterialSelection()
    {
        if (materialArived)
        {
            Color baseColor = materialMaker.transform.GetChild(0).GetChild(4).GetComponent<Image>().color;
            Color emmisonColor = materialMaker.transform.GetChild(1).GetChild(4).GetComponent<Image>().color;
            float metallic = materialMaker.transform.GetChild(2).GetChild(0).GetComponent<Slider>().value;
            float smooth = materialMaker.transform.GetChild(3).GetChild(0).GetComponent<Slider>().value;
            materialMaker.SetActive(false);
            materialSelecting = false;
            commands += baseColor.r + "," + baseColor.g + "," + baseColor.b + "," + baseColor.a + "," + metallic + "," + smooth + "," + emmisonColor.r + "," + emmisonColor.g + "," + emmisonColor.b + "," + emmisonColor.a + "}];";
            materialArived = false;
        }
    }


    protected override void openUI()
    {
        if (!UIManager.UIOpen)
        {
            path = null;
            lastMadeObject = null;
            Destroy(component.transform.GetChild(1).gameObject);
            GameObject newDisplay = new GameObject("Display");
            newDisplay.transform.SetParent(component.transform);
            newDisplay.transform.localScale = Vector3.one;
            newDisplay.transform.localPosition = Vector3.zero;
            Destroy(component.transform.GetChild(1).gameObject);
            materialMaker = Instantiate(materialMakerPrefab, component.transform);
        }
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
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                commands += "[AOS{0,0,0}];";
                GameObject temp = new GameObject("AOS");
                temp.AddComponent<Image>().sprite = UIMagicHandler.AOS;
                temp.transform.SetParent (component.transform.GetChild(1));
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
            materialMaker.SetActive(true);
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

        if (earlyReturn())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            isOpen = onExit();
            toggleOn();
        }

        if (path != null)
        {
            HandleKeyBinds();
        }
    }
}
