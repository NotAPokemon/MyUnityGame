using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skitter : Mob
{
    // Start is called before the first frame update
    protected override void runBehaviour()
    {
        if (isMad())
        {
            if (isFacingTarget())
            {
                HandleMove(10, 10);
            }
            else
            {
                turn();
            }
        }
        if (!isGrounded())
        {
            yVelocity -= gravity * Time.deltaTime;
            HandleMove(0, 0);
        }

        if (getDistanceToTarget() <= 8 && getDistanceToTarget() >= 2.3)
        {
            SkitterJump();
        } else if (getDistanceToTarget() <= 8)
        {
            target.health -= damage();
        }

    }

    void SkitterJump()
    {
        if (isGrounded())
        {
            yVelocity = Mathf.Sqrt(jumpHight * 2f * gravity);
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            HandleMove(directionToTarget.x * 15, directionToTarget.z * 15);
        }
    }


    protected override bool isMad()
    {
        return getDistanceToTarget() <= 30;
    }

    protected override void Update()
    {
        base.Update();
        runBehaviour();
    }



}
