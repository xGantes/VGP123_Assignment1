using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;
    //public Vector2 followOffSet;
    private Vector2 thold;
    public float speed = 5f;
    private Rigidbody2D rb;

    //clamping
    public Vector3 minValue, maxValue;
    // Start is called before the first frame update
    void Start()
    {
       // thold = calcthold();
        rb = followObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 follow = followObject.transform.position;
        float xdiff = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float ydiff = Vector2.Distance(Vector2.up * transform.position.x, Vector2.up * follow.x);

        Vector3 newPos = transform.position;

        if (Mathf.Abs(xdiff) >= thold.x)
        {
            newPos.x = follow.x;
        }
        if (Mathf.Abs(ydiff) >= thold.y)
        {
            newPos.y = follow.y;
        }
        float ms = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;

        //bound position or limiting
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(newPos.x, minValue.x, maxValue.x),
            Mathf.Clamp(newPos.y, minValue.y, maxValue.y),
            Mathf.Clamp(newPos.z, minValue.z, maxValue.z));

        //transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, boundPosition, speed * Time.deltaTime);
    }
    // Update is called once per frame
    /* void Update()
     {
     } */
    /*
    private Vector3 calcthold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffSet.x;
        t.y -= followOffSet.y;
        return t;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calcthold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    } */
}