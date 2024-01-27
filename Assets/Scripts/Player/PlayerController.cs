using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    /* Base Stats */
    public float moveSpeed = 1f;
    [SerializeField]
    private int currentSanity = 30;
    public int maxSanity = 100;
    [SerializeField]
    private int SanityRestoreOnBossKill = 5;
    [SerializeField]
    private ResourceBar sanityBar;   
    private bool isSane = true;   
    [SerializeField]
    private Image sanityIcon;
    [Space()]

    [SerializeField]
    private Sprite veryLowSanityImage;
    [SerializeField]
    private Sprite lowSanityImage;
    [SerializeField]
    private Sprite mediumSanityImage;
    [SerializeField]
    private Sprite highSanityImage;
    [SerializeField]
    private Sprite veryHighSanityImage;

    [Space()]
    public bool IsMovementPaused = false;
    [SerializeField]
    private LevelScript levelScript;

    public Animator animator;
    //private Coroutine gameOverSequenceCoroutine;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D playerRigidbody;  

    public bool IsSane { get { return isSane; } }

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        sanityBar.SetMaxValue(maxSanity);
        sanityBar.SetValue(currentSanity);

        levelScript.bossKillEvent.AddListener(BossKillHeal);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isSane || IsMovementPaused)
            return;

        playerRigidbody.MovePosition((Vector2)transform.position + moveDirection * moveSpeed);
    }

    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>().normalized;
        
    }

    private void BossKillHeal()
    {
        ChangeHealth(SanityRestoreOnBossKill);
    }

    public void ChangeHealth(int value)
    {
        if (!isSane)
            return;

        currentSanity = Mathf.Clamp(currentSanity + value, 0, maxSanity);
        sanityBar.SetValue(currentSanity);

        UpdateSanityIcon();

        if (currentSanity <= 0)
        {
            levelScript.GameOver();
            isSane = false;
        }
        else if (currentSanity >= maxSanity)
        {
            levelScript.GameWin();
        }
            

    }

    private void UpdateSanityIcon()
    {
        if ((float)currentSanity / maxSanity < 0.2f)
            sanityIcon.sprite = veryLowSanityImage;
        else if ((float)currentSanity / maxSanity < 0.4f)
            sanityIcon.sprite = lowSanityImage;
        else if ((float)currentSanity / maxSanity < 0.6f)
            sanityIcon.sprite = mediumSanityImage;
        else if ((float)currentSanity / maxSanity < 0.8f)
            sanityIcon.sprite = highSanityImage;
        else
            sanityIcon.sprite = veryHighSanityImage;


    }
}
