using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mob : BaseEntity
{
    public BaseEntity target;

    
    public float speed = 1.0f;
    public float jumpHight = 2.0f;
    Vector3 move;
    protected float gravity = 9.81f * 2;
    protected float yVelocity = 0f;
    public float damageCD;
    protected float timeSinceLastDmg;
    float timeSinceHurt;
    float timeSinceSpawn = 0;
    Material main;

    public int level;
    public float experiance;


    protected override void Start()
    {
        base.Start();
        timeSinceLastDmg = 0;
        main = transform.GetComponentInChildren<MeshRenderer>().material;
        MaxHealth *= level;
        health *= level;
        damageAmount *= level;
        health = MaxHealth;
    }


    public float damage()
    {
        if (timeSinceLastDmg >= damageCD)
        {
            timeSinceLastDmg = 0;
            return damageAmount;
        }
        else
        {
            return 0;
        }
    }

    protected void HandleJump()
    {
     

        if (isGrounded())
        {
            yVelocity = Mathf.Sqrt(jumpHight * 2.5f * gravity);
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }
    }


    


    public virtual void handleSpawn()
    {
        experiance = Random.Range(Calculator.calculateExperiance(level-1), Calculator.calculateExperiance(level)-1);
    }


    protected override void handleDeath()
    {
        if (timeSinceSpawn < 1)
        {
            health = MaxHealth;
            return;
        }
        base.handleDeath();
        float div = Calculator.randomDiv(1,50);
        Player.player.exp += experiance / div;
    }

    protected virtual void idle()
    {
        move = transform.right * Random.Range(-10,10) + transform.forward * Random.Range(-10,10);
        move.y = yVelocity;
        Controller.Move(move * speed * Time.deltaTime);
    }


    protected void HandleMove(float x, float z)
    {
        move = transform.right * x + transform.forward * z;
        move.y = yVelocity;
        Controller.Move(move * speed * Time.deltaTime);
    }

    protected override void HandleDamage(float amount)
    {
        base.HandleDamage(amount);
        transform.GetComponentInChildren<MeshRenderer>().material = EntityManager.hurtMat;
        timeSinceHurt = 0;

    }


    protected bool isFacingTarget()
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;


        Vector3 forwardDirection = transform.forward;

        float dotProduct = Vector3.Dot(forwardDirection, directionToTarget);

        
       return dotProduct > 0.5f;
    }

    protected float getDistanceToTarget()
    {

        return Vector3.Distance(transform.position, target.transform.position);

    }

    protected void turn()
    {

        transform.Rotate(0, 80 * Time.deltaTime, 0);
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        HandleMove(directionToTarget.x * speed * -speed, directionToTarget.z * speed * -speed);
    }

    protected virtual void runBehaviour() { }
   
    protected virtual bool isMad() { return false; }



    protected override void Update() 
    {
        base.Update();
        timeSinceLastDmg += Time.deltaTime;
        timeSinceHurt += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceHurt >= 0.25)
        {
            transform.GetComponentInChildren<MeshRenderer>().material = main;
        }
    }
}
