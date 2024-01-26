using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth = 10;
    [SerializeField]
    private float deathDuration = 1f;
    /*[SerializeField]
    private string deathSound; when audio manager kicks in */
    //public UnityEvent enemyDeathEvent = new(); track deaths??

    /* Movement */
    [SerializeField]
    protected float baseSpeed = 0.1f;
    private bool isMovementPaused = false; //enemy movement paused (saves lookX)
    protected Vector2 moveDirection = Vector2.zero; //direction of movement, also used for determining sprite direction for overrides

    /* Enemy Hit */
    [Space()]
    private Material baseMaterial;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();

        baseMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if not dead, do sprite movement change
        if (isAlive)
        {
            if (!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);
        }
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

    protected virtual void Move()
    {
        Vector2 position = (Vector2)transform.position + (baseSpeed * moveDirection);
        enemyRigidbody.MovePosition(position);
    }

    public void ChangeHealth(int value)
    {
        Debug.Log($"changing enemy health by {value}");

        if (!isAlive)
            return;

        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        /*if (healthBar != null)
            healthBar.SetHealth(currentHealth);
        */

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
            EnemyDeath();
    }

    private void EnemyDeath()
    {
        //AudioManager.Instance.Play(deathSound);
        //enemyDeathEvent.Invoke();
        isAlive = false;
        animator.SetTrigger("death");
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        spriteRenderer.material = baseMaterial;
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
    }

    IEnumerator FlashEffect()
    {
        WaitForSeconds flashDuration = new(this.flashDuration);

        spriteRenderer.material = flashMaterial;

        yield return flashDuration;

        spriteRenderer.material = baseMaterial;
    }
}
