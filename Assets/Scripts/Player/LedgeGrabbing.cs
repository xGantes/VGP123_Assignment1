using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask WhatsGround;

    private bool gBox, rBox;
    public float rOffSetX, rOffSetY, rOffSetSizeX, rOffSetSizeY, gOffSetX, gOffSetY, gOffSetSizeY, gOffSetSizeX;
    private float gravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
    }
    private void Update()
    {
        LedgeGrab();
    }

    void LedgeGrab()
    {
        if (PlayerControls.isRunning && PlayerControls.isJumping)
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

            if (gBox && !rBox && !PlayerControls.isGrabbing && PlayerControls.isJumping)
            {
                PlayerControls.isGrabbing = true;
            }
            if (rBox)
            {
                PlayerControls.isGrabbing = false;
            }
            if (PlayerControls.isGrabbing)
            {
                rb.velocity = new Vector2(0f, 1f);
                rb.gravityScale = 0f;
            }
        }
    }
    public void ChangePos()
    {
        transform.position = new Vector2
        (
            transform.position.x + (0.5f * transform.localScale.x),
            transform.position.y + 0.4f
        );
        rb.gravityScale = gravity;
        PlayerControls.isGrabbing = false;
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
