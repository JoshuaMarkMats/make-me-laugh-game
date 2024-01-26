using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    /* Attack */
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float detectionRange = 2f;
    private bool isAttacking = false;
    [SerializeField]
    private string attackSound;
    [SerializeField]
    private AttackArea attackArea;

    [Space()]
    [SerializeField]
    private Transform pivotParent;

    private EnemyMelee enemyController;
    private Animator animator;

    private const string ATTACK_TRIGGER = "attack";
    private const string FINISH_ATTACK_TRIGGER = "finishAttack";

    void Start()
    {
        enemyController= GetComponent<EnemyMelee>();
        animator = GetComponent<Animator>();
        //enemyController.enemyDeathEvent.AddListener(CancelAttack);
    }

    void Update()
    {
        if (!isAttacking && enemyController.IsAlive)
        {
            float aimAngle = Vector2.SignedAngle(Vector2.right, enemyController.VectorToTarget.normalized);
            pivotParent.eulerAngles = new Vector3(0, 0, aimAngle);

            if (enemyController.VectorToTarget.sqrMagnitude <= detectionRange * detectionRange)
            {
                enemyController.IsMovementPaused = true;
                isAttacking = true;
                animator.SetTrigger(ATTACK_TRIGGER);
            }
        }            
    }

    /*private void OnDrawGizmosSelected()
    {
        //attack areas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.left * attackAreaRange, attackAreaRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.right * attackAreaRange, attackAreaRadius);
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset, detectionRange);
    }*/

    private void DoDamage()
    {
        //AudioManager.Instance.Play(attackSound);

        /*attackDirection = (enemyController.LookDirection < 0) ? Vector2.left : Vector2.right;

        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position + attackCenterOffset + attackDirection * attackAreaRange, attackAreaRadius, attackableLayers);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ChangeHealth(-damage);
            }
        }*/

        attackArea.Attack(damage);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        enemyController.IsMovementPaused = false;
        animator.SetTrigger(FINISH_ATTACK_TRIGGER);
    }
}
