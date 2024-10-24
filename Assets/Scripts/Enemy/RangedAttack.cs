using UnityEngine;

public class RangedAttack : MeleeAttack
{
    [SerializeField]
    private Vector2 attackCenter = Vector2.up;
    public ObjectPool bulletPool;
    [SerializeField]
    private float bulletSpeed;

    protected override void Update()
    {
        if (!isAttacking && enemyController.IsAlive)
        {
            //Debug.Log($"{gameObject.name} is getting ready to attack");
            if (enemyController.VectorToTarget.sqrMagnitude <= detectionRange * detectionRange)
            {
                //Debug.Log($"{gameObject.name} is attacking");
                enemyController.IsMovementPaused = true;
                isAttacking = true;
                animator.Play(ATTACK_STATE);
            }
        }            
    }

    private void OnDrawGizmosSelected()
    {
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenter, detectionRange);
    }

    protected override void DoDamage()
    {
        PoolableObject poolableObject= bulletPool.GetObject();

        if (poolableObject == null)
            return;

        if (poolableObject.TryGetComponent<Bullet>(out var bullet))
        {
            AudioManager.Instance.Play(attackSound);

            bullet.speed = bulletSpeed;
            bullet.damage = damage;

            Vector2 vectorToTarget = (Vector2)enemyController.target.position - ((Vector2)transform.position + attackCenter);
            bullet.direction = vectorToTarget.normalized;
            bullet.transform.position = (Vector2)transform.position + attackCenter;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, bullet.direction));
        }
    }
}
