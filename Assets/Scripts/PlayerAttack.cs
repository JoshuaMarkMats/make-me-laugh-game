using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform aimIndicator;

    /*float aimAngle = 0f;
    [SerializeField]
    private float aimConeAngle = 30f;
    public float attackRange = 5f;*/
    [Space()]

    [SerializeField]
    private float currentStamina;
    [SerializeField]
    private float maxStamina = 50;
    [SerializeField]
    private float staminaRegen = 1f;
    [SerializeField]
    private ResourceBar staminaBar;

    [Space()]

    [SerializeField]
    private int giggleDamage = 2;
    [SerializeField]
    private float giggleCooldown = 0.5f;
    [SerializeField]
    private AttackArea giggleAttackArea;
    private float giggleCurrentCooldown = 0;
    [SerializeField]
    private float giggleStaminaCost = 2;
    [Space()]
    [SerializeField]
    private int laughDamage = 4;
    [SerializeField]
    private float laughCooldown = 3f;
    [SerializeField]
    private AttackArea laughAttackArea;
    private float laughCurrentCooldown = 0;
    [SerializeField]
    private float laughStaminaCost = 10;
    [Space()]
    [SerializeField]
    private int boisterousLaughDamage = 5;
    [SerializeField]
    private float boisterousLaughCooldown = 7.5f;
    [SerializeField]
    private AttackArea boisterousLaughAttackArea;
    private float boisterousLaughCurrentCooldown = 0;
    [SerializeField]
    private float boisterousLaughStaminaCost = 25;

    [Space()]

    [SerializeField]
    private GameObject pivotParent; //parent object to allow children to have diff pivot points for rotation


    /*void OnDrawGizmos()
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
    }*/

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxValue(maxStamina);
    }

    void Update()
    {
        float aimAngle = Vector2.SignedAngle(Vector2.right, GetAimDirection());
        pivotParent.transform.eulerAngles = new Vector3(0, 0, aimAngle);

        ManageTimers();

        Debug.Log($"Giggle: {(giggleCooldown - giggleCurrentCooldown > 0 ? giggleCooldown - giggleCurrentCooldown : "Ready")} | Laugh: {(laughCooldown - laughCurrentCooldown > 0 ? laughCooldown - laughCurrentCooldown : "Ready")} | Boisterous Laugh: {(boisterousLaughCooldown - boisterousLaughCurrentCooldown > 0 ? boisterousLaughCooldown - boisterousLaughCurrentCooldown : "Ready")}");
    }

    void ManageTimers()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * staminaRegen;
            staminaBar.SetValue(currentStamina);
        }


        if (giggleCurrentCooldown < giggleCooldown)
            giggleCurrentCooldown += Time.deltaTime;
        if (laughCurrentCooldown < laughCooldown)
            laughCurrentCooldown += Time.deltaTime;
        if (boisterousLaughCurrentCooldown < boisterousLaughCooldown)
            boisterousLaughCurrentCooldown += Time.deltaTime;
    }

    void OnGiggle()
    {
        if (giggleCurrentCooldown < giggleCooldown || currentStamina < giggleStaminaCost)
            return;
        giggleAttackArea.Attack(giggleDamage);
        giggleCurrentCooldown = 0;
        
        currentStamina -= giggleStaminaCost;
    }

    void OnLaugh()
    {
        if (laughCurrentCooldown < laughCooldown || currentStamina < laughStaminaCost)
            return;
        
        laughAttackArea.Attack(laughDamage);
        laughCurrentCooldown = 0;

        currentStamina -= laughStaminaCost;
    }

    void OnBoisterousLaugh()
    {
        if (boisterousLaughCurrentCooldown < boisterousLaughCooldown || currentStamina < boisterousLaughStaminaCost)
            return;

        boisterousLaughAttackArea.Attack(boisterousLaughDamage);
        boisterousLaughCurrentCooldown = 0;

        currentStamina -= boisterousLaughStaminaCost;
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        return (mousePos - playerPos).normalized;
    }
}
