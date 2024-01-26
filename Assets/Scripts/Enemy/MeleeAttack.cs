using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    /* Attack */
    [SerializeField]
    private int damage = 10;
    /*[SerializeField]
    private float windupDuration = 0.4f;
    [SerializeField]
    private float attackDuration = 1f;*/
    [SerializeField]
    private float detectionRange = 2f;
    [SerializeField]
    private float attackAreaRange = 2f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 3f; //radius of attack area
    [SerializeField]
    private Vector2 attackCenterOffset = Vector2.up; //offset for center of attack stuff
    private Vector2 attackDirection;
    [SerializeField]
    private LayerMask attackableLayers;
    private bool isAttacking = false;
    [SerializeField]
    private string attackSound;

    private ChasingEnemy enemyController;
    private Animator animator;

    private const string ATTACK_TRIGGER = "attack";
    private const string FINISH_ATTACK_TRIGGER = "finishAttack";

    void Start()
    {
        enemyController= GetComponent<ChasingEnemy>();
        animator = GetComponent<Animator>();
        //enemyController.enemyDeathEvent.AddListener(CancelAttack);
    }

    void Update()
    {

        if (!isAttacking && enemyController.IsAlive && Physics2D.OverlapCircle((Vector2)transform.position + attackCenterOffset, detectionRange, attackableLayers) != null)
        {
            enemyController.IsMovementPaused = true;
            isAttacking = true;
            animator.SetTrigger(ATTACK_TRIGGER);
        }
            
    }

    private void OnDrawGizmosSelected()
    {
        //attack areas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.left * attackAreaRange, attackAreaRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.right * attackAreaRange, attackAreaRadius);
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset, detectionRange);
    }

    private void DoDamage()
    {
        //AudioManager.Instance.Play(attackSound);

        attackDirection = (enemyController.LookDirection < 0) ? Vector2.left : Vector2.right;

        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position + attackCenterOffset + attackDirection * attackAreaRange, attackAreaRadius, attackableLayers);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ChangeHealth(-damage);
            }
        }       
    }

    private void FinishAttack()
    {
        isAttacking = false;
        enemyController.IsMovementPaused = false;
        animator.SetTrigger(FINISH_ATTACK_TRIGGER);
    }

    /*private void CancelAttack()
    {
        if (attackCoroutine != null) 
            StopCoroutine(attackCoroutine);
    }*/
}
