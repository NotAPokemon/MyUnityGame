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



    protected override void Start()
    {
        base.Start();
        timeSinceLastDmg = 0;
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





    protected void HandleMove(float x, float z)
    {
        move = transform.right * x + transform.forward * z;
        move.y = yVelocity;
        Controller.Move(move * speed * Time.deltaTime);
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
    }
}
