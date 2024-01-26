using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gift : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GiftType giftType;
    [SerializeField]
    private GameObject interactIcon;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public GiftType GiftType
    {
        get { return giftType; }
    }

    private void Awake()
    {
        textMeshProUGUI.text = $"Pick up {giftType.GiftName}";
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = giftType.GiftIcon;
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
