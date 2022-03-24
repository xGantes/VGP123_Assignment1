using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoar : MonoBehaviour
{
    public static float damage;

    private void Update()
    {
        if (PlayerControls.isRoaring)
        {
            damage = 10;
        }
    }
}
