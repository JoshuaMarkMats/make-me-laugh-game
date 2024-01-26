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
    public int meleeEnemyCap = 40;
    private int meleeAlive = 0;

    [Space()]

    private ObjectPool rangedPool;
    [SerializeField]
    private PoolableObject rangedEnemy;
    public int rangedEnemyCap = 40;
    private int rangedAlive = 0;

    [Space()]

    private ObjectPool bossPool;
    private PoolableObject bossEnemy;
    public int bossEnemyCap = 20;

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

    public Transform player;
    
    

    [SerializeField]
    public static UnityEvent meleeEnemyDeathEvent = new();

    public void Awake()
    {
        meleePool = ObjectPool.CreateInstance(meleeEnemy, meleeEnemyCap);
        rangedPool = ObjectPool.CreateInstance(rangedEnemy, rangedEnemyCap);
        //ObjectPool.CreateInstance(bossEnemy, bossEnemyCap);

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

        while (true)
        {
            int meleeToSpawn = currentWave; // x
            int spawnedMelee = 0;

            //Debug.Log($"Trying to spawn {meleeToSpawn} melee");
            while (meleeAlive < meleeEnemyCap && spawnedMelee < meleeToSpawn)
            {
                DoSpawnMelee();

                spawnedMelee++;

                yield return IntervalWait;
            }

            int rangedToSpawn = currentWave/2; // x/2
            int spawnedRanged = 0;

            while (rangedAlive < rangedEnemyCap && spawnedRanged < rangedToSpawn)
            {
                DoSpawnRanged();

                spawnedRanged++;

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
        //enemy.transform.position = new Vector2((float)Mathf.Cos(Random.Range(0f, 3.1415f)) * spawnDistance, (float)Mathf.Sin(Random.Range(0f, 3.1415f)) * spawnDistance);
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

        meleeAlive++;

        RangedAttack enemyAttack = poolableObject.GetComponent<RangedAttack>();
        Enemy enemy = poolableObject.GetComponent<Enemy>();

        enemy.target = player;
        enemy.IsAlive = true;
        enemy.IsMovementPaused = false;
        enemyAttack.bulletPool = bulletPool;
        //enemy.transform.position = new Vector2((float)Mathf.Cos(Random.Range(0f, 3.1415f)) * spawnDistance, (float)Mathf.Sin(Random.Range(0f, 3.1415f)) * spawnDistance);
        enemy.transform.position = Random.insideUnitCircle.normalized * spawnDistance; 
        enemy.enemyDeathEvent.AddListener(ReduceRangedCount);
        enemy.gameObject.SetActive(true);
    }

    private void ReduceRangedCount()
    {
        rangedAlive--;
    }
}
