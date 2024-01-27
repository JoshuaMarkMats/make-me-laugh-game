using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Child : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private int sanityIncrease = 5;
    [Space()]
    [SerializeField]
    private GameObject interactIcon;

    private Animator childAnimator;

    private const string LAUGH_TRIGGER = "laugh";
    private const string DEATH_TRIGGER = "death";
    private const string HAPPY_TRIGGER = "happy";

    private void Awake()
    {
        childAnimator = GetComponent<Animator>();
    }

    public void GiveGift()
    {
        player.ChangeHealth(sanityIncrease);
        childAnimator.SetTrigger(LAUGH_TRIGGER);
    }

    public void Kill()
    {
        childAnimator.SetTrigger(DEATH_TRIGGER);
    }

    public void MakeHappy()
    {
        childAnimator.SetTrigger(HAPPY_TRIGGER);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactIcon.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactIcon.SetActive(false);
    }
}
