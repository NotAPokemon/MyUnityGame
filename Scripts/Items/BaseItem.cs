using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public ItemState State;
    GameObject item;
    public bool GroundAnimationDirection;
    public float damage = 10;
    protected bool doingAnimaton = false;
    public float attackTime = 0.5f;
    protected float time;

    

    protected virtual void Start()
    {
        item = transform.gameObject;
    }

    protected virtual void rightClick()
    {

    }

    protected virtual void clickAnimation()
    {
        if (time  >= attackTime)
        {
            time = 0;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            doingAnimaton = false;
        } 
        else if (time >= attackTime/2)
        {
            transform.Rotate(-Time.deltaTime * 20 / (attackTime * attackTime), 0, 0);
            time += Time.deltaTime;
        } else
        {
            transform.Rotate(Time.deltaTime * 20 / (attackTime * attackTime), 0, 0);
            time += Time.deltaTime;
        }
    }

    protected virtual void leftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 2, out hit))
        {
            if (Vector3.Distance(hit.transform.position, transform.position) <= 3)
            {
                BaseEntity hitEntity = hit.transform.GetComponent<BaseEntity>();
                if (hitEntity != null)
                {
                    hitEntity.health -= (damage + FindObjectOfType<Player>().damageAmount);
                }
            }
        }
    }

    private float GetDistanceToLayerBelow(Vector3 origin)
    {
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, Mathf.Infinity);
        float distance = Vector3.Distance(origin, hit.point);
        return distance;
    }

    protected virtual void groundAnimation()
    {
        float amount = 0.3f;
        float distanceBellow = GetDistanceToLayerBelow(item.transform.position);
        if (distanceBellow >= 0.5)
        {
            GroundAnimationDirection = true;
            
        } else if (distanceBellow <= 0.1)
        {
            GroundAnimationDirection = false;
        }
        if (distanceBellow >= 0.7)
        {
            amount *= 9.81f * 2f;
        }

        amount *= GroundAnimationDirection ? -1 : 1;
        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + amount * Time.deltaTime, item.transform.position.z);
        item.transform.Rotate(new Vector3(0, amount, 0));
    }

    protected virtual void Update()
    {
        if (State == ItemState.Hand) {
            if (doingAnimaton)
            {
                clickAnimation();
            }
            item.SetActive(true);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!doingAnimaton)
                {
                    leftClick();
                    doingAnimaton = true;
                    clickAnimation();
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                rightClick();
            }

        } else if (State == ItemState.Ground)
        {
            item.SetActive(true);
            groundAnimation();

        } else if (State == ItemState.Stored)
        {
            item.SetActive(false);
        }
    }
}

public enum ItemState { Stored, Ground, Hand };
