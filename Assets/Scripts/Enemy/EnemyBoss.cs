using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    private float baseSpeed;
    public LevelScript LevelScript;

    protected override void Start()
    {
        base.Start();

        baseSpeed = moveSpeed;
    }

    protected override void EnemyDeath()
    {
        base.EnemyDeath();

        LevelScript.bossKillEvent.Invoke();
    }

    //the following stuff's cuz of the odd walking
    private void PauseMovement()
    {
        moveSpeed = 0;
    }

    private void ResumeMovement()
    {
        moveSpeed = baseSpeed;
    }
}
