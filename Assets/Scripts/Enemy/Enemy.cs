using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    private CircleCollider2D[] colliders;
    private BoxCollider2D[] boxcolliders;
    //floats
    public float Speed;
    public float Force;

    //bools
    public bool isAttacking;
    public bool isPatroling;
    public bool isFlipping;
    public bool isSquished;
    public bool isDying;

    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        //enemy health
        if (maxHealth <= 0)
        {
            maxHealth = 100;
        }
        currentHealth = maxHealth;
    }

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<CircleCollider2D>();
        boxcolliders = GetComponents<BoxCollider2D>();
    }
    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            currentHealth -= PlayerRoar.damage;
            Debug.Log("Damage hit");
        }
        if(collision.tag == "StompHitbox")
        {
            if (PlayerControls.isJumping)
            {
                //Debug.Log("Stomp!");
                Death();
                SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.stomp);
            }
        }
    }

    private void Death()
    {
        animator.SetTrigger("death");

        foreach (var boxCollider2D in boxcolliders)
        {
            boxCollider2D.enabled = false;
        }

        Wait(() =>
        {
            Destroy(gameObject);
        }, 2f);
    }

    public void Wait(Action action, float delay)
    {
        StartCoroutine(WaitCoroutine(action, delay));
    }

    IEnumerator WaitCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);

        action();
    }
}
