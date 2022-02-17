using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectableObjects : MonoBehaviour
{
    public Collectables[] CollectablesArray;

    private void Start()
    {
        Instantiate(CollectablesArray[0], transform.position, transform.rotation);
    }
}
