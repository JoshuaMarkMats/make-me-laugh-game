using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private GiftSlot giftSlot;

    [SerializeField]
    private float interactRange = 2f;
    [SerializeField]
    private Vector2 interactOffset = Vector2.up;
    [SerializeField]
    private LayerMask giftLayer;
    [SerializeField]
    private LayerMask childLayer;

    private PlayerController playerController;

    private void OnDrawGizmosSelected()
    {
        //interaction range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + interactOffset, interactRange);
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnInteract()
    {
        if (!playerController.IsSane)
            return;

        //rip redundant checks if current gift exists
        if (giftSlot.CurrentGift != null)
        {
            Collider2D collider2d = Physics2D.OverlapCircle((Vector2)transform.position + interactOffset, interactRange, childLayer);
            if (collider2d == null)
                return;

            Child child = collider2d.GetComponent<Child>();
            if (child != null)
            {
                if (giftSlot.RemoveGift())
                    child.GiveGift();
            }
        }
        else
        {
            Collider2D focusItem = Physics2D.OverlapCircle((Vector2)transform.position + interactOffset, interactRange, giftLayer);
            if (focusItem == null)
                return;

            Gift gift = focusItem.GetComponent<Gift>();
            if (gift != null)
            {
                if (giftSlot.AddGift(gift.GiftType))
                    Destroy(focusItem.gameObject);
            }            
        }
            
    }
}
