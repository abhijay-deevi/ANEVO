using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerControl : MonoBehaviour
    
{

    public float speed = 5;
    public Rigidbody2D rb;

    [HideInInspector] public Vector2 lastMovement = Vector2.down; // Default Position
    void FixedUpdate()
    {
        float horizontal, vertical;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);
        rb.linearVelocity = movement * speed;

        // Updates last direction
        if (movement != Vector2.zero)
        {
            lastMovement = movement.normalized; // Always length 1
        }
    }
}
