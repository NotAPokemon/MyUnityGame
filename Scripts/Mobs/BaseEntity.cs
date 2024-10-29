using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float MaxHealth;
    public float health;
    public float damageAmount;
    public float damageCD;
    protected float timeSinceLastDmg;
    public Vector2 chunkLocation;
    public GameObject GroundCheck;
    public float groundDistance = 0.2f;
    public LayerMask ground;
    public CharacterController Controller;
    public LootData[] drops;

    bool DeathCanceld = false;

    bool dead = false;

    protected virtual void Start()
    {
        Controller = transform.GetComponent<CharacterController>();
        timeSinceLastDmg = 0;
        health = MaxHealth;
    }
    protected bool isGrounded()
    {
        return Physics.CheckSphere(GroundCheck.transform.position, groundDistance, ground);
    }

    public float damage()
    {
        if(timeSinceLastDmg >= damageCD)
        {
            timeSinceLastDmg = 0;
            return damageAmount;
        }
        else
        {
            return 0;
        }
    }

    public void cancelDeath()
    {
        DeathCanceld = true;
    }

    public void uncancelDeath()
    {
        DeathCanceld = false;
    }

    protected virtual void handleDeath()
    {
        if (!DeathCanceld)
        {
            for (int i = 0; i < drops.Length; i++)
            {
                for (int j = 0; j < drops[i].amount; j++)
                {
                    if (EntityManager.ChanceOf(drops[i].dropRate))
                    {
                        GameObject item = Instantiate(drops[i].item.transform.gameObject);
                        item.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

                        item.GetComponent<BaseItem>().State = ItemState.Ground;
                        item.transform.SetParent(FindObjectOfType<EntityManager>().transform);
                        item.SetActive(true);
                    }
                }
            }
            Destroy(gameObject);
        } else
        {
            health = MaxHealth;
            dead = false;
        }
        
    }

    protected virtual void Update()
    {
        timeSinceLastDmg += Time.deltaTime;
        if (transform.position.y < -200)
        {
            Destroy(gameObject);
        }
        if ( health <= 0 )
        {
            dead = true;
        }
        if (dead)
        {
            handleDeath();
        }
    }



}

[Serializable]
public struct LootData
{
    public BaseItem item;
    public float dropRate;
    public float amount;
}
