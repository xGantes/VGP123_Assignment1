using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]

public class PlayerControls : MonoBehaviour
{
    [SerializeField][HideInInspector] private float Speed;
    [SerializeField][HideInInspector] private float JumpForce;
    [SerializeField][HideInInspector] private float GroundCheckRadius;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask WhatsGround;

    private Rigidbody2D rb;
    private Light inspector;

    public PhysicsMaterial2D withFriction;
    public PhysicsMaterial2D noFriction;
    //private SpriteRenderer sr;

    Animator animator;
    private string currentState;
    const string Player_Idle = "Player_Idle";
    const string Player_Run = "Player_Run";
    const string Player_Jump = "Player_Jump";
    const string Player_StandLand = "Player_StandLand";
    const string Player_RunJump = "Player_RunJump";
    const string Player_LedgeGrab = "Player_LedgeGrab";
    const string Player_Roar = "Player_Roar";

    //bools
    public bool isGrounded;
    public bool verbose = false;
    private bool isFacingRight = true;
    public static bool isJumping, isRunning, isGrabbing, isLanding, isRoaring;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //sr = GetComponent<SpriteRenderer>();

        //animation entry
        animator = GetComponentInChildren<Animator>();
        animator.Play("Player_Idle");

        //roarArea = transform.GetChild(1).gameObject;

        //character's behavior
        if(Speed <= 0)
        {
            Speed = 9.0f;
        }
        if(JumpForce <= 0)
        {
            JumpForce = 16f;
        }
        if (GroundCheckRadius <= 0)
        {
            GroundCheckRadius = 0.10f;
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

        try
        {
            inspector.color = Color.red;
        }
        catch (NullReferenceException)
        {
            Debug.Log("Player Control's Light was not set in the inspector");
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
        Roaring();

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrabbing", isGrabbing);

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
                rb.sharedMaterial = noFriction;
                isRunning = true;
            }
            else
            {
                rb.sharedMaterial = withFriction;
                isRunning = false;
            }
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
            rb.AddForce(Vector2.up * JumpForce);
        }
    }

    void Roaring()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!isRoaring && !isJumping)
            {
                isRoaring = true;
                SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.roar);
            }
        }
    }

    void PlayerAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(Player_LedgeGrab) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            GetComponent<LedgeGrabbing>().ChangePos();
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
}
