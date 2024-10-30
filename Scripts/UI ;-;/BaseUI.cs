using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Sprite CreateSpriteFromGameObject(GameObject targetObject)
    {
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Texture2D texture = renderer.material.mainTexture as Texture2D;

            if (texture != null)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);
                return sprite;
            }
            else
            {
                Debug.LogWarning("No texture found on the object's material.");
            }
        }
        else
        {
            Debug.LogWarning("No Renderer found on the target GameObject.");
        }

        return null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        component.SetActive(isOpen);
        if (Input.GetKeyDown(keyCode))
        {
            openUI();
            toggleOn();
        }
    }
}
