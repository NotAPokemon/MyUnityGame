using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{


    public bool Locked = true;
    public float MouseRotationX = 0f;
    public float MouseRotationY = 0f;
    public float MouseSensitvity = 0f;
    Player player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = FindObjectOfType<Player>();
    }


    void MouseMovment()
    {
        float MouseHorizontal = Input.GetAxis("Mouse X");
        float MouseVertical = Input.GetAxis("Mouse Y");
        MouseRotationX += MouseHorizontal;
        MouseRotationY -= MouseVertical;
        MouseRotationY = Mathf.Clamp(MouseRotationY, -70f, 90f);
        transform.localRotation = Quaternion.Euler(MouseRotationY, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(0, MouseRotationX, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Locked = !Locked;
        }

        if (Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            MouseMovment();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
