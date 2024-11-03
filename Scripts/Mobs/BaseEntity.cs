using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float MaxHealth;
    public float health;
    public float damageAmount;
    public Vector2 chunkLocation;
    public GameObject GroundCheck;
    public float groundDistance = 0.2f;
    public LayerMask ground;
    public CharacterController Controller;
    public LootData[] drops;
    protected float defense;

    protected float lastHealth;

    bool DeathCanceld = false;

    bool dead = false;

    protected virtual void Start()
    {
        Controller = transform.GetComponent<CharacterController>();
        health = MaxHealth;
    }
    protected bool isGrounded()
    {
        return Physics.CheckSphere(GroundCheck.transform.position, groundDistance, ground);
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
                        item.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-2, 2), transform.position.y + 1, transform.position.z + UnityEngine.Random.Range(-2,2));

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

    protected virtual void HandleDamage(float amount)
    {
        health += amount * (Mathf.Exp(-((defense * Mathf.Log10(100)) / (100000))));
        health = Mathf.Clamp(health, 0f, MaxHealth);
    }



    protected virtual void Update()
    {
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
        if (lastHealth > health)
        {
            HandleDamage(lastHealth - health);
            lastHealth = health;
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
