using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask WhatsGround;

    private Rigidbody2D rb;
    //private SpriteRenderer sr;

    Animator animator;
    private string currentState;
    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Run";
    const string Player_Jump = "Player_Jump";
    const string Player_RunJump = "Player_RunJump";
    const string Player_LedgeGrab = "Player_LedgeGrab";
    const string Player_Roar = "Player_Roar";

    //bools
    public bool isGrounded;
    public bool verbose = false;
    private bool isFacingRight = true;
    public static bool isJumping, isRunning, isGrabbing, isRoaring;

    private bool gBox, rBox;
    public float rOffSetX, rOffSetY, rOffSetSizeX, rOffSetSizeY, gOffSetX, gOffSetY, gOffSetSizeY, gOffSetSizeX;
    private float gravity;

    private float AttackTimer = 0f;
    private float AttackDuration = 0.10f;
    private GameObject Attacking = default;
    //jumps
    //private float JumpCounter;
    //public float JumpTime;

    #region
    int _lives = 0;
    int _stamina = 0;
    int _hp = 5;

    public int maxLives = 3;
    public int maxStamina = 10;
    public int maxHp = 10;

    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
            if(_lives > maxLives)
            {
                _lives = maxLives;
            }
            Debug.Log("Lives set to: " + lives.ToString());
        }
    }
    public int stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = value;
            if(_stamina > maxStamina)
            {
                _stamina = maxStamina;
            }
            Debug.Log("Stamina set to: " + stamina.ToString());
        }
    }
    public int health
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(_hp > maxHp)
            {
                _hp = maxHp;
            }
            Debug.Log("Health is set to: " + health.ToString());
        }
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //sr = GetComponent<SpriteRenderer>();

        //animation entry
        animator = GetComponentInChildren<Animator>();
        animator.Play("Player_Idle");

        gravity = rb.gravityScale;

        Attacking = transform.GetChild(0).gameObject;

        //character's behavior
        if(Speed <= 0)
        {
            Speed = 12.0f;
        }
        if(JumpForce <= 0)
        {
            JumpForce = 16f;
        }
        if (GroundCheckRadius <= 0)
        {
            GroundCheckRadius = 0.04f;
        }
        if(!GroundCheck)
        {
            GroundCheck = transform.GetChild(0);
            if(verbose)
            {
                if(GroundCheck.name == "GroundCheck")
                {
                    Debug.Log("Ground is found");
                }
                else
                {
                    Debug.Log("Ground is not found");
                }
            }
        }
    }
    void ChangeAnimatorState(string newState)
    {
        //stop an animation
        if (currentState == newState) return;

        //play animation
        animator.Play(newState);

        //reaasign animation
        currentState = newState;
    }

    void Update()
    {
        HoriMovement();
        JumpMovement();
        PlayerAnimation();
        LedgeGrab();
        Roaring();

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrabbing", isGrabbing);
        
        /*
        AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip[0].clip.name != "isFiring")
        {
            Vector2 directMove = new Vector2(HoriMovement * Speed,
            rb.velocity.y);
            rb.velocity = directMove;
        }
        else
        {
            rb.velocity = Vector2.zero;
        } */

        
    }
    void HoriMovement()
    {
        if (!isGrabbing && !isRoaring)
        {
            //xV controls
            float HoriMovement = Input.GetAxisRaw("Horizontal");
            isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatsGround);

            Vector2 dirMove = new Vector2(HoriMovement * Speed, rb.velocity.y);
            rb.velocity = dirMove;

            //new
            if (HoriMovement != 0)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
            /*
            if (HoriMovement > 0 && sr.flipX || HoriMovement < 0 && !sr.flipX)
            {
                sr.flipX = !sr.flipX;
            } */
            if(HoriMovement > 0 && !isFacingRight || HoriMovement < 0 && isFacingRight)
            {
                Vector3 currentScale = gameObject.transform.localScale;
                currentScale.x *= -1;
                gameObject.transform.localScale = currentScale;

                isFacingRight = !isFacingRight;
            }
            animator.SetFloat("Speed", Mathf.Abs(HoriMovement));
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    void JumpMovement()
    {
        isJumping = !isGrounded;

        if(Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    void LedgeGrab()
    {
        gBox = Physics2D.OverlapBox
        (
            new Vector2(transform.position.x + (gOffSetX * transform.localScale.x),
            transform.position.y + gOffSetY),
            new Vector2(gOffSetSizeX, gOffSetSizeY), 0f, WhatsGround
        );
        rBox = Physics2D.OverlapBox
        (
            new Vector2(transform.position.x + (rOffSetX * transform.localScale.x),
            transform.position.y + rOffSetY),
            new Vector2(rOffSetSizeX, rOffSetSizeY), 0f, WhatsGround
        );

        if (gBox && !rBox && !isGrabbing && isJumping)
        {
            isGrabbing = true;
        }
        if(isGrabbing)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
        }

    }
    void ChangePos()
    {
        transform.position = new Vector2
        (
            transform.position.x + (0.5f * transform.localScale.x), 
            transform.position.y + 0.4f
        );
        rb.gravityScale = gravity;
        isGrabbing = false;
    }
    public void Roaring()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(!isJumping)
            {
                if(!isRoaring)
                {
                    isRoaring = true;
                    animator.SetTrigger("isRoaring");
                }
            }
            if (isRoaring)
            {
                AttackTimer += Time.deltaTime;
                if(AttackTimer >= AttackDuration)
                {
                    AttackTimer = 0;
                    isRoaring = false;
                    Attacking.SetActive(Attacking);
                }
            }
           
        }
        
    }
    void PlayerAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(Player_LedgeGrab) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ChangePos();
        }
        if (isRunning && !isJumping && !isRoaring)
        {
            ChangeAnimatorState(Player_Run);
        }
        else if(!isRunning && !isJumping && !isRoaring)
        {
            ChangeAnimatorState(Player_Idle);
        }
        else if(isJumping && !isGrabbing)
        {
            if (isRunning)
            {
                ChangeAnimatorState(Player_RunJump);
            }
            else
            {
                ChangeAnimatorState(Player_Jump);
            }
        }
        else if (isGrabbing)
        {
            ChangeAnimatorState(Player_LedgeGrab);
        }
        else if(isRoaring)
        {
            ChangeAnimatorState(Player_Roar);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(Player_Roar) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                isRoaring = false;
            }
        }


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (gOffSetX * transform.localScale.x),
            transform.position.y + gOffSetY),
            new Vector2(gOffSetSizeX, gOffSetSizeY));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (rOffSetX * transform.localScale.x),
            transform.position.y + rOffSetY),
            new Vector2(rOffSetSizeX, rOffSetSizeY));
    }
}
