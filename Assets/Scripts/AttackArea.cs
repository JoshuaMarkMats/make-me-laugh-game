using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Collider2D areaCollider;
    private List<Collider2D> hitTargets = new();

    private void Awake()
    {
        areaCollider = GetComponent<Collider2D>();
    }

    public void Attack(int damage)
    {
        areaCollider.Overlap(hitTargets);

        foreach (Collider2D target in hitTargets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                Debug.Log($"Attacked {target.gameObject.name} for {damage}");
                damageable.ChangeHealth(-damage);
            }
            
        }
    }
}
