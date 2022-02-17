using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    public Transform GroundCheckPos;
    public LayerMask WhatsGround;
    public Collider2D WallCollider;

    //floats
    public float Speed;

    //bools
    public bool isPatroling;
    public bool isFlipping;

    private void Start()
    {
        isPatroling = true;
        if(Speed <= 0)
        {
            Speed = 70.0f;
        }

    }
    private void Update()
    {

        if(isPatroling)
        {
            rb.velocity = new Vector2(Speed * Time.fixedDeltaTime, rb.velocity.y);
            if(isFlipping || WallCollider.IsTouchingLayers(WhatsGround))
            {
                Flips();
            }
        }
    }
    private void FixedUpdate()
    {
        if(isPatroling)
        {
            isFlipping = !Physics2D.OverlapCircle(GroundCheckPos.position, 0.2f, WhatsGround);
        }
    }
    void Flips()
    {
        isPatroling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        Speed *= -1;
        isPatroling = true;
    }
}
