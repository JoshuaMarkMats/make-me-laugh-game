using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
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

        Collider2D collider = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);
        if (collider == null)
            return;

        IInteractable interactable = collider.GetComponent<IInteractable>();
        if (interactable != null)
            interactable.OnInteract();
    }
}
