using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform aimIndicator;

    [Space()]

    [SerializeField]
    private float currentStamina = 50;
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
    private float giggleCurrentCooldown;
    [SerializeField]
    private float giggleStaminaCost = 2;
    [SerializeField]
    private string giggleSound = "Giggle";
    [SerializeField]
    private Slider giggleCooldownSlider;
    [SerializeField]
    private TextMeshProUGUI giggleCooldownText;
    [Space()]
    [SerializeField]
    private int laughDamage = 4;
    [SerializeField]
    private float laughCooldown = 3f;
    [SerializeField]
    private AttackArea laughAttackArea;
    private float laughCurrentCooldown;
    [SerializeField]
    private float laughStaminaCost = 10;
    [SerializeField]
    private string laughSound = "Laugh";
    [SerializeField]
    private Slider laughCooldownSlider;
    [SerializeField]
    private TextMeshProUGUI laughCooldownText;
    [Space()]
    [SerializeField]
    private int boisterousLaughDamage = 5;
    [SerializeField]
    private float boisterousLaughCooldown = 7.5f;
    [SerializeField]
    private AttackArea boisterousLaughAttackArea;
    private float boisterousLaughCurrentCooldown;
    [SerializeField]
    private float boisterousLaughStaminaCost = 25;
    [SerializeField]
    private string boisterousLaughSound = "BoisterousLaugh";
    [SerializeField]
    private Slider boisterousLaughCooldownSlider;
    [SerializeField]
    private TextMeshProUGUI boisterousLaughCooldownText;

    [Space()]

    private bool isAttacking = false;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject pivotParent; //parent object to allow children to have diff pivot points for rotation

    private const string GIGGLE_TRIGGER = "giggle";
    private const string LAUGH_TRIGGER = "laugh";
    private const string BOISTEROUS_LAUGH_TRIGGER = "boisterousLaugh";


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
        playerController = GetComponent<PlayerController>();

        staminaBar.SetMaxValue(maxStamina);
        staminaBar.SetValue(currentStamina);

        giggleCurrentCooldown = giggleCooldown;
        laughCurrentCooldown = laughCooldown;
        boisterousLaughCurrentCooldown = boisterousLaughCooldown;

        giggleCooldownSlider.maxValue = giggleCooldown;
        laughCooldownSlider.maxValue = laughCooldown;
        boisterousLaughCooldownSlider.maxValue = boisterousLaughCooldown;
    }

    void Update()
    {
        //only do aiming if not in the middle of an attack
        if (!isAttacking && playerController.IsSane)
        {
            float aimAngle = Vector2.SignedAngle(Vector2.right, GetAimDirection());
            pivotParent.transform.eulerAngles = new Vector3(0, 0, aimAngle);
        }
        

        ManageTimers();
    }

    void ManageTimers()
    {
        if (!playerController.IsSane)
            return;

        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * staminaRegen;
            staminaBar.SetValue(currentStamina);
        }


        if (giggleCurrentCooldown > 0)
        {
            giggleCurrentCooldown -= Time.deltaTime;
            //update slider
            giggleCooldownSlider.value = giggleCurrentCooldown;
            //after adding, check if we should still display cooldown text
            giggleCooldownText.text = (giggleCurrentCooldown > 0) ? $"{(int)giggleCurrentCooldown}s" : "";
        }
            
        if (laughCurrentCooldown > 0)
        {
            laughCurrentCooldown -= Time.deltaTime;
            laughCooldownSlider.value = laughCurrentCooldown;
            laughCooldownText.text = (laughCurrentCooldown > 0) ? $"{(int)laughCurrentCooldown}s" : "";
        }
            
        if (boisterousLaughCurrentCooldown > 0)
        {
            boisterousLaughCurrentCooldown -= Time.deltaTime;
            boisterousLaughCooldownSlider.value = boisterousLaughCurrentCooldown;
            boisterousLaughCooldownText.text = (boisterousLaughCurrentCooldown > 0) ? $"{(int)boisterousLaughCurrentCooldown}s" : "";
        }
           
    }

    void OnGiggle()
    {
        if (giggleCurrentCooldown > 0 || currentStamina < giggleStaminaCost || isAttacking || !playerController.IsSane)
            return;

        AudioManager.Instance.Play(giggleSound);

        isAttacking = true;
        playerController.IsMovementPaused = true;
        playerController.animator.SetTrigger(GIGGLE_TRIGGER);

        giggleAttackArea.Attack(giggleDamage);
        giggleCurrentCooldown = giggleCooldown;
        
        currentStamina -= giggleStaminaCost;
    }

    void OnLaugh()
    {
        if (laughCurrentCooldown > 0 || currentStamina < laughStaminaCost || isAttacking || !playerController.IsSane)
            return;

        AudioManager.Instance.Play(laughSound);

        isAttacking = true;
        playerController.IsMovementPaused = true;
        playerController.animator.SetTrigger(LAUGH_TRIGGER);

        laughAttackArea.Attack(laughDamage);
        laughCurrentCooldown = laughCooldown;

        currentStamina -= laughStaminaCost;
    }

    void OnBoisterousLaugh()
    {
        if (boisterousLaughCurrentCooldown > 0 || currentStamina < boisterousLaughStaminaCost || isAttacking || !playerController.IsSane)
            return;

        AudioManager.Instance.Play(boisterousLaughSound);

        isAttacking = true;
        playerController.IsMovementPaused = true;
        playerController.animator.SetTrigger(BOISTEROUS_LAUGH_TRIGGER);

        boisterousLaughAttackArea.Attack(boisterousLaughDamage);
        boisterousLaughCurrentCooldown = boisterousLaughCooldown;

        currentStamina -= boisterousLaughStaminaCost;
    }

    private void FinishAttack()
    {
        isAttacking = false;
        playerController.IsMovementPaused = false;
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        return (mousePos - playerPos).normalized;
    }
}
