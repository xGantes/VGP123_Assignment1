using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    enum CollectibleType
    {
        Life,
        Stamina,
        Hp,
    }

    [SerializeField] CollectibleType collects;
    private Rigidbody2D rb;
    public int GameStatus;

    private void Start()
    {
        /*
        if(collects == CollectibleType.Life)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(4, rb.velocity.y);
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.soundInstances.audio.PlayOneShot(SoundManager.soundInstances.cool);
            //GameManager playerStats = collision.gameObject.GetComponent<GameManager>();
            switch (collects)
            {
                case CollectibleType.Hp:
                    //playerStats.health++;
                    GameManager.instances.health++;
                    break;
                case CollectibleType.Life:
                    //playerStats.lives++;
                    GameManager.instances.lives++;
                    break;
                case CollectibleType.Stamina:
                    //playerStats.stamina++;
                    GameManager.instances.stamina++;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
