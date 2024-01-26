using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{   
    public Transform target;
    private Vector2 vectorToTarget;

    public Vector2 VectorToTarget { get { return vectorToTarget; } }

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            //update pathfinding
            vectorToTarget = target.position - transform.position;
            moveDirection = vectorToTarget.normalized;
        }
        else
            moveDirection = Vector2.zero;
        

    }
}
