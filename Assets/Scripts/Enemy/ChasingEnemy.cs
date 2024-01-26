using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyBase
{   
    [SerializeField]
    private float aggroRange = 5f;
    [SerializeField]
    private LayerMask aggroMask;

    private Collider2D target;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    protected override void Update()
    {
        base.Update();

        target = Physics2D.OverlapCircle(transform.position, aggroRange, aggroMask);
        if (target != null)
        {
            //update pathfinding
            moveDirection = target.transform.position - transform.position;
            moveDirection.Normalize();
        }
        else
            moveDirection = Vector2.zero;
        

    }
}
