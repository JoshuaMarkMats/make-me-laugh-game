using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Base Stats */
    [SerializeField]
    private int currentSanity;
    public int maxSanity = 100;
    [SerializeField]
    private ResourceBar sanityBar;
    [Space()]

    public float moveSpeed = 1f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D playerRigidbody;

    
    

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerRigidbody.MovePosition((Vector2)transform.position + moveDirection * moveSpeed);
    }

    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>().normalized;
        
    }
}
