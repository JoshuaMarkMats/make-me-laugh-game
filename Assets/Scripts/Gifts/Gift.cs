using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gift : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    private GiftType giftType;
    [SerializeField]
    private GameObject interactIcon;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public GiftType GiftType { 
        get { return giftType; } 
        set
        {
            giftType = value;
            spriteRenderer.sprite = value.GiftIcon;
            textMeshProUGUI.text = $"Pick up {giftType.GiftName}";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactIcon.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactIcon.SetActive(false);
    }

    public void OnInteract()
    {
         Destroy(gameObject);
    }
}
