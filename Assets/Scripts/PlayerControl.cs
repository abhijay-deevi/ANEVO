using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
    
{

    public float speed = 5;
    public Rigidbody2D rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal, vertical;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(horizontal,vertical)*speed;
    }
}
