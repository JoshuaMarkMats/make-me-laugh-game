using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    protected override void EnemyDeath()
    {
        base.EnemyDeath();

        LevelScript.bossKillEvent.Invoke();
    }
}
