using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    /* Attack */
    [SerializeField]
    protected int damage = 10;
    [SerializeField]
    protected float detectionRange = 2f;
    protected bool isAttacking = false;
    [SerializeField]
    private string attackSound;
    [SerializeField]
    private AttackArea attackArea;

    [Space()]
    [SerializeField]
    private Transform pivotParent;

    protected Enemy enemyController;
    protected Animator animator;

    protected const string ATTACK_TRIGGER = "attack";
    private const string FINISH_ATTACK_TRIGGER = "finishAttack";

    void Start()
    {
        enemyController= GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        //enemyController.enemyDeathEvent.AddListener(CancelAttack);
    }

    protected virtual void Update()
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

    private void OnDrawGizmosSelected()
    {
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position, detectionRange);
    }

    protected virtual void DoDamage()
    {
        //AudioManager.Instance.Play(attackSound);

        attackArea.Attack(damage);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        enemyController.IsMovementPaused = false;
        animator.SetTrigger(FINISH_ATTACK_TRIGGER);
    }
}
