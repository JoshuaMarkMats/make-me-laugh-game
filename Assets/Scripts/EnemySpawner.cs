using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool meleePool;
    private PoolableObject meleeEnemy;
    public int meleeEnemyCap = 40;
    private int meleeAlive = 0;

    [Space()]

    private ObjectPool bossPool;
    private PoolableObject bossEnemy;
    public int bossEnemyCap = 20;
    [Space()]
    public float SpawnDelay = 5f;
    private float SpawnMaxDelay = 15f;
    private float SpawnDelayIncrease = 0.1f;
    [Space()]
    private float spawnDistance = 5f;
    private int currentWave = 1;

    public Transform player;
    
    

    [SerializeField]
    public static UnityEvent meleeEnemyDeathEvent = new();

    public void Awake()
    {
        ObjectPool.CreateInstance(meleeEnemy, meleeEnemyCap);
        ObjectPool.CreateInstance(bossEnemy, bossEnemyCap);
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new(SpawnDelay);

        int meleeToSpawn = currentWave * 2 - 1; //2x-1
        int spawnedMelee = 0;

        while (meleeAlive < meleeEnemyCap && spawnedMelee < meleeToSpawn)
        {
            DoSpawnMelee();

            spawnedMelee++;

            
        }

        yield return Wait;
    }

    private void DoSpawnMelee()
    {
        PoolableObject poolableObject = meleePool.GetObject();

        if (poolableObject == null)
            return;

        meleeAlive++;

        Enemy enemy = poolableObject.GetComponent<Enemy>();

        enemy.target = player;
        enemy.transform.position = new Vector2((float)Mathf.Cos(Random.Range(0f, 3.1415f)) * spawnDistance, (float)Mathf.Sin(Random.Range(0f, 3.1415f)) * spawnDistance);
        enemy.enemyDeathEvent.AddListener(ReduceMeleeCount);
        enemy.gameObject.SetActive(true);
    }

    private void ReduceMeleeCount()
    {
        meleeAlive--;
    }
}
