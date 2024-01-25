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

    public void Attack()
    {
        areaCollider.Overlap(hitTargets);

        foreach (Collider2D collider in hitTargets)
        {
            Debug.Log("Attacked: " + collider.gameObject.name);
        }
    }
}
