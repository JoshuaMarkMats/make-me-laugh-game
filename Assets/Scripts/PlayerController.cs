using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    /* Base Stats */
    [SerializeField]
    private int currentSanity = 30;
    public int maxSanity = 100;
    [SerializeField]
    private ResourceBar sanityBar;
    [Space()]
    private bool isSane = true;

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
        sanityBar.SetMaxValue(maxSanity);
        sanityBar.SetValue(currentSanity);
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

    public void ChangeHealth(int value)
    {

        if (!isSane)
            return;

        /*if (value < 0)
        {
            if (isInvincible)
            {
                Debug.Log("Invincible");
                return;
            }


            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlayerHurt();
            invincibleBlinkCoroutine = StartCoroutine(InvincibleBlink(invincibleBlinkInterval));
        }*/

        currentSanity = Mathf.Clamp(currentSanity + value, 0, maxSanity);
        sanityBar.SetValue(currentSanity);

        if (currentSanity <= 0)
           GameOver();

    }

    private void GameOver()
    {

    }
}
