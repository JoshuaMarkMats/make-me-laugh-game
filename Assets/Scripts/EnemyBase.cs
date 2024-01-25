using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    /* Enemy Hit */
    [Space()]
    private Material baseMaterial;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

    //private bool isAlive = true;
    //private Animator animator;
    //private float lookDirection = 1;
    //protected Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(int value)
    {
        /*if (!isAlive)
            return;

        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
        */

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashEffect());

        //if (currentHealth <= 0)
        //    EnemyDeath();
    }

    IEnumerator FlashEffect()
    {
        WaitForSeconds flashDuration = new(this.flashDuration);

        spriteRenderer.material = flashMaterial;

        yield return flashDuration;

        spriteRenderer.material = baseMaterial;
    }
}
