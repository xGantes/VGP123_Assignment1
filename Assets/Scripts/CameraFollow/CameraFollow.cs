using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;

    public float minXClamp = -3.3f;
    public float maxXClamp = 44.4f;
    public float minYClamp = -6.2f;
    public float maxYClamp = 39.2f;

    private void LateUpdate()
    {
        if (GameManager.instances.playerInstances)
        {
            Vector3 xCamTransform = transform.position;
            xCamTransform.x = GameManager.instances.playerInstances.transform.position.x;
            xCamTransform.x = Mathf.Clamp(xCamTransform.x, minXClamp, maxXClamp);
            transform.position = xCamTransform;

            Vector3 yCamTransform = transform.position;
            yCamTransform.y = GameManager.instances.playerInstances.transform.position.y;
            yCamTransform.y = Mathf.Clamp(yCamTransform.y, minYClamp, maxYClamp);
            transform.position = yCamTransform;
        }
    }
}