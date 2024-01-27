using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool meleePool;
    [SerializeField]
    private PoolableObject meleeEnemy;
    [SerializeField]
    private int meleeEnemyCap = 40;
    private int meleeAlive = 0;

    [Space()]

    private ObjectPool rangedPool;
    [SerializeField]
    private PoolableObject rangedEnemy;
    [SerializeField]
    private int rangedEnemyCap = 40;
    private int rangedAlive = 0;

    [Space()]

    private ObjectPool bossPool;
    [SerializeField]
    private PoolableObject bossEnemy;
    [SerializeField]
    private int bossEnemyCap = 20;
    private int bossAlive = 0;
    [SerializeField]
    private string bossSpawnSound = "BossSpawn";

    [Space()]

    private ObjectPool bulletPool;
    [SerializeField]
    private PoolableObject bullet;
    private int bulletCap = 50;

    [Space()]

    [SerializeField]
    private float SpawnDelay = 10f;
    [SerializeField]
    private float SpawnMaxDelay = 25f;
    [SerializeField]
    private float SpawnDelayIncrease = 0.5f;
    [SerializeField]
    private float SpawnInterval = 0.1f; //time delay between individual spawns
    [Space()]
    [SerializeField]
    private float spawnDistance = 5f;
    private int currentWave = 1;

    [Space()]

    public Transform player;
    [SerializeField]
    private LevelScript levelScript;
    public static UnityEvent meleeEnemyDeathEvent = new();

    public void Awake()
    {
        meleePool = ObjectPool.CreateInstance(meleeEnemy, meleeEnemyCap);
        rangedPool = ObjectPool.CreateInstance(rangedEnemy, rangedEnemyCap);
        bossPool = ObjectPool.CreateInstance(bossEnemy, bossEnemyCap);

        bulletPool = ObjectPool.CreateInstance(bullet, bulletCap);
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnDrawGizmosSelected()
    {
        //spawn area
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position, spawnDistance);
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new(SpawnDelay);
        WaitForSeconds IntervalWait = new(SpawnInterval);

        while (!levelScript.IsGameWon)
        {
            /* MELEE */

            int meleeToSpawn = currentWave; // x
            int spawnedMelee = 0;        

            while (meleeAlive < meleeEnemyCap && spawnedMelee < meleeToSpawn)
            {
                DoSpawnMelee();

                spawnedMelee++;

                yield return IntervalWait;
            }

            /* RANGED */

            int rangedToSpawn = currentWave/2; // x/2
            int spawnedRanged = 0;

            while (rangedAlive < rangedEnemyCap && spawnedRanged < rangedToSpawn)
            {
                DoSpawnRanged();

                spawnedRanged++;

                yield return IntervalWait;
            }

            /* BOSS */

            int bossToSpawn = currentWave / 5; // x/5
            int spawnedBoss = 0;

            while (bossAlive < bossEnemyCap && spawnedBoss < bossToSpawn && currentWave % 5 == 0) //last condition ensure bosses only spawn every 5 waves
            {
                DoSpawnBoss();

                spawnedBoss++;

                yield return IntervalWait;
            }

            yield return Wait;
            currentWave++;

            if (SpawnDelay < SpawnMaxDelay)
                SpawnDelay += SpawnDelayIncrease;
        }
        
    }

    private void DoSpawnMelee()
    {
        PoolableObject poolableObject = meleePool.GetObject();

        if (poolableObject == null)
            return;

        meleeAlive++;

        Enemy enemy = poolableObject.GetComponent<Enemy>();

        enemy.target = player;
        enemy.IsAlive = true;
        enemy.IsMovementPaused = false;
        levelScript.clearEnemiesEvent.AddListener(enemy.Kill);
        enemy.transform.position = Random.insideUnitCircle.normalized * spawnDistance;
        enemy.enemyDeathEvent.AddListener(ReduceMeleeCount);
        enemy.gameObject.SetActive(true);
    }

    private void ReduceMeleeCount()
    {
        meleeAlive--;
    }

    private void DoSpawnRanged()
    {
        PoolableObject poolableObject = rangedPool.GetObject();

        if (poolableObject == null)
            return;

        rangedAlive++;

        RangedAttack enemyAttack = poolableObject.GetComponent<RangedAttack>();
        Enemy enemy = poolableObject.GetComponent<Enemy>();

        enemy.target = player;
        enemy.IsAlive = true;
        enemy.IsMovementPaused = false;
        enemyAttack.bulletPool = bulletPool;
        levelScript.clearEnemiesEvent.AddListener(enemy.Kill);
        enemy.transform.position = Random.insideUnitCircle.normalized * spawnDistance; 
        enemy.enemyDeathEvent.AddListener(ReduceRangedCount);
        enemy.gameObject.SetActive(true);
    }

    private void ReduceRangedCount()
    {
        rangedAlive--;
    }

    private void DoSpawnBoss()
    {
        PoolableObject poolableObject = bossPool.GetObject();

        if (poolableObject == null)
            return;

        bossAlive++;

        EnemyBoss enemy = poolableObject.GetComponent<EnemyBoss>();

        AudioManager.Instance.Play(bossSpawnSound);

        enemy.target = player;
        enemy.IsAlive = true;
        enemy.IsMovementPaused = false;
        enemy.LevelScript = levelScript;
        levelScript.clearEnemiesEvent.AddListener(enemy.Kill);
        enemy.transform.position = Random.insideUnitCircle.normalized * spawnDistance;
        enemy.enemyDeathEvent.AddListener(ReduceBossCount);
        enemy.gameObject.SetActive(true);
    }

    private void ReduceBossCount()
    {
        bossAlive--;
    }
}
