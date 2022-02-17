using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoars : MonoBehaviour
{
    public float Speed;
    public float RoarDuration;

    Rigidbody2D rb;

    void Start()
    {
        if (RoarDuration <= 0)
        {
            RoarDuration = 0.05f;
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(Speed, 0);
        Destroy(gameObject, RoarDuration);
    }
}
