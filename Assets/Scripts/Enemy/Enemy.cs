using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Enemy : PoolableObject, IDamageable
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth = 10;
    [SerializeField]
    private float deathDuration = 1f;
    /*[SerializeField]
    private string deathSound; when audio manager kicks in */
    public UnityEvent enemyDeathEvent = new();
    [Space()]

    /* Movement */
    [SerializeField]
    protected float baseSpeed = 0.1f;
    [SerializeField]
    private bool isMovementPaused = false; //enemy movement paused (saves lookX)
    protected Vector2 moveDirection = Vector2.zero; //direction of movement, also used for determining sprite direction for overrides

    //chasing
    [Space()]
    public Transform target;
    private Vector2 vectorToTarget;
    public Vector2 VectorToTarget { get { return vectorToTarget; } }

    /* Enemy Hit */
    [Space()]
    private Material baseMaterial;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

    [SerializeField]
    private bool isAlive = true;
    private Animator animator;
    private float lookDirection = 1;
    protected Rigidbody2D enemyRigidbody;
    private SpriteRenderer spriteRenderer;

    public float LookDirection { get { return lookDirection; } }
    public bool IsAlive
    {
        get { return isAlive; }
        set
        {
            if (value == false)
                EnemyDeath();
        }
    }
    public bool IsMovementPaused { get { return isMovementPaused; } set { isMovementPaused = value; } }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();

        baseMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        //if not dead, do sprite movement change
        if (isAlive)
        {
            if (!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);
        }

        if (target != null)
        {
            //update pathfinding
            vectorToTarget = target.position - transform.position;
            moveDirection = vectorToTarget.normalized;
        }
        else
            moveDirection = Vector2.zero;
    }

    private void LateUpdate()
    {
        //if paused or dead, don't do direction changes
        if (isMovementPaused || !isAlive)
            return;

        //sprite direction
        if (!Mathf.Approximately(moveDirection.x, 0.0f))
            lookDirection = moveDirection.x;
        animator.SetFloat("lookX", lookDirection);
    }

    private void FixedUpdate()
    {
        if (isAlive && !isMovementPaused)
            Move();
    }

    private void Move()
    {
        Vector2 position = (Vector2)transform.position + (baseSpeed * moveDirection);
        enemyRigidbody.MovePosition(position);
    }

    public void ChangeHealth(int value)
    {
        //Debug.Log($"changing {gameObject.name} health by {value}");

        if (!isAlive)
            return;

        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
            EnemyDeath();
    }

    protected virtual void EnemyDeath()
    {
        //AudioManager.Instance.Play(deathSound);
        //Debug.Log($"{gameObject.name} dies");
        
        isAlive = false;
        animator.SetTrigger("death");
        /*if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);*/
        spriteRenderer.material = baseMaterial;
    }

    private void RemoveEnemy()
    {
        enemyDeathEvent.Invoke();
        gameObject.SetActive(false);       
    }

    IEnumerator FlashEffect()
    {
        WaitForSeconds flashDuration = new(this.flashDuration);

        spriteRenderer.material = flashMaterial;

        yield return flashDuration;

        spriteRenderer.material = baseMaterial;
    }
}
