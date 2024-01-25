using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform aimIndicator;

    float aimAngle = 0f;
    [SerializeField]
    private float aimConeAngle = 30f;
    public float attackRange = 5f;

    [SerializeField]
    private GameObject pivotParent; //parent object to allow children to have diff pivot points for rotation
    [SerializeField]
    private AttackArea attackArea;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the gizmo color

        // Draw the cone
        DrawCone(transform.position, transform.right, aimConeAngle, attackRange);
    }

    void DrawCone(Vector3 position, Vector3 direction, float angle, float range)
    {
        // Calculate cone vertices
        Vector3 coneBase1 = position + Quaternion.Euler(0, 0, aimAngle - angle / 2) * direction * range;
        Vector3 coneBase2 = position + Quaternion.Euler(0, 0, aimAngle + angle / 2) * direction * range;

        // Draw lines to visualize the cone
        Gizmos.DrawLine(position, coneBase1);
        Gizmos.DrawLine(position, coneBase2);
        Gizmos.DrawLine(coneBase1, coneBase2);
    }

    void Update()
    {
        aimAngle = Vector2.SignedAngle(Vector2.right, GetAimDirection());
        pivotParent.transform.eulerAngles = new Vector3(0, 0, aimAngle);
    }

    void OnFire()
    {
        attackArea.Attack();
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        return (mousePos - playerPos).normalized;
    }
}
