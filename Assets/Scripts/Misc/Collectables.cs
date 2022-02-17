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
            PlayerControls playerCon = collision.gameObject.GetComponent<PlayerControls>();
            switch (collects)
            {
                case CollectibleType.Hp:
                    playerCon.health++;
                    break;
                case CollectibleType.Life:
                    playerCon.lives++;
                    break;
                case CollectibleType.Stamina:
                    playerCon.stamina++;
                    break;
            }
            Destroy(gameObject);
        }
    }

}
