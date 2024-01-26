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
    private LayerMask interactableLayer;

    private void OnDrawGizmosSelected()
    {
        //interaction range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + interactOffset, interactRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInteract()
    {
        Debug.Log("interacting");

        Collider2D focusItem = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);
        if (focusItem == null)
            return;

        Gift gift = focusItem.GetComponent<Gift>();
        if (gift != null)
        {
            if (giftSlot.AddGift(gift.GiftType))
                Destroy(focusItem.gameObject);
        }
        else
        {
            Child child = focusItem.GetComponent<Child>();
            if (child != null)
            {
                child.GiveGift();
                giftSlot.RemoveGift();
            }
        }
            
    }
}
