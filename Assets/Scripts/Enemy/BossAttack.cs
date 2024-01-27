using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossAttack : MonoBehaviour
{
    /* Main Stats */
    [SerializeField]
    private float detectionRange = 2f;
    [Space()]

    /* Basic Attack */
    [SerializeField]
    private int basicDamage = 10;
    [SerializeField]
    private string basicAttackSound;
    [SerializeField]
    private AttackArea basicAttackArea;
    [Space()]

    /* AoE Attack */
    [SerializeField]
    private int aoeDamage = 10;
    [SerializeField]
    private string aoeAttackSound;
    [SerializeField]
    private AttackArea aoeAttackArea;
    [Space()]

    [SerializeField]
    private Transform pivotParent;

    private bool isAttacking;

    private Enemy enemyController;
    private Animator animator;

    private const string BASIC_ATTACK_TRIGGER = "basicAttack";
    private const string AOE_ATTACK_TRIGGER = "aoeAttack";

    void Start()
    {
        enemyController= GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isAttacking= false;
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
                animator.SetTrigger(Random.value < 0.5f ? BASIC_ATTACK_TRIGGER : AOE_ATTACK_TRIGGER);
            }
        }            
    }

    private void OnDrawGizmosSelected()
    {
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position, detectionRange);
    }

    private void DoBasicDamage()
    {
        //AudioManager.Instance.Play(attackSound);

        basicAttackArea.Attack(basicDamage);
    }

    private void DoAoeDamage()
    {
        //AudioManager.Instance.Play(attackSound);

        aoeAttackArea.Attack(aoeDamage);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        enemyController.IsMovementPaused = false;
    }
}
