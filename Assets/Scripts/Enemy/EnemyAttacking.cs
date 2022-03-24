using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public Transform cast;
    public Transform leftLimit;
    public Transform rightLimit;
    public LayerMask castMask;
    public float castLength;
    public float attackDist;
    public float speed;
    public float timer;

    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    Rigidbody2D rb;
    private float distance, attackRange;
    private bool attackMode;
    private bool isRange;
    private bool isCooling;
    private float isIntime;

    public static int damageToPlayer;

    private void Awake()
    {
        SelectTarget();
        isIntime = timer;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        damageToPlayer = 10;
    }
    private void Update()
    {
        if(!attackMode)
        {
            Move();
        }
        if (!insideLimit() && !isRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Chameleon_Attack"))
        {
            SelectTarget();
        }
        if(isRange)
        {
            hit = Physics2D.Raycast(cast.position, transform.right, castLength, castMask);
            castDebug();
        }
        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            isRange = false;
        }
        if (isRange == false)
        {
            //anim.SetBool("walk",false);
            attackStopped();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance > attackDist)
        {
            //Move();
            attackStopped();
        }
        else if(attackDist >= distance && isCooling == false)
        {
            attack();
        }
        if(isCooling)
        {
            cooldown();
            anim.SetBool("attack", false);
        }
    }
    void Move()
    {
        anim.SetBool("walk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Chameleon_Attack"))
        {
            if (target != null)
            {
                Vector2 tp = new Vector2(target.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, tp, speed * Time.deltaTime);
            }
            else
            {
                SelectTarget();
            }
        }
    }
    void attack()
    {
        timer = isIntime;
        attackMode = true;

        anim.SetBool("walk", false);
        anim.SetBool("attack", true);
    }

    void attackStopped()
    {
        isCooling = false;
        attackMode = false;
        anim.SetBool("attack", false);
    }

    void cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && isCooling && attackMode)
        {
            isCooling = false;
            timer = isIntime;
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            target = trigger.transform;
            isRange = true;
            Flip();
        }
        if (trigger.tag == "EnemyHitbox")
        {
            Debug.Log("Got Hit!");
            GameManager.instances.health--;
            //GameManager gameManagerTrigg = trigger.GetComponent<GameManager>();
            //if (gameManagerTrigg)
            //{
            //    gameManagerTrigg.Damage(damageToPlayer);
            //}
            SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.chamAtk);
        }
    }

    void castDebug()
    {
        if(distance > attackDist)
        {
            Debug.DrawRay(cast.position, transform.right * castLength, Color.red);
        }
        else if(distance < attackDist)
        {
            Debug.DrawRay(cast.position, transform.right * castLength, Color.green);
        }
    }

    void triggerCooling()
    {
        isCooling = true;
    }

    private bool insideLimit()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    void SelectTarget()
    {
        float disToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float disToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(disToLeft > disToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }
        Flip();
    }
    void Flip()
    {
        if (GameManager.instances.playerInstances)
        {
            Vector3 rotation = transform.eulerAngles;
            if (transform.position.x > target.transform.position.x)
            {
                rotation.y = 180f;
            }
            else
            {
                rotation.y = 0f;
            }
            transform.eulerAngles = rotation;
        }
    }
}
