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

    public void GiveGift()
    {
        player.ChangeHealth(sanityIncrease);
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
