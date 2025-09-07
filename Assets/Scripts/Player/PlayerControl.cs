using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
    
{

    public float speed = 5;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement; // stores x, y

    // Health Stuff
    private Health health;
    public Image healthBar;


    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void OnEnable() // Setting up Observer Pattern
    {
        health.OnDied += HandleDeath;
        health.OnHealthChanged += HandleHealthChanged;
    }

    void OnDisable() // Remove from observer pattern
    {
        health.OnDied -= HandleDeath;
        health.OnHealthChanged -= HandleHealthChanged;
    }

    private void Update()
    {
        if (health.isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // performance trick
    }

    private void FixedUpdate()
    {
        if (health.isDead) return;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void HandleDeath()
    {
        movement = Vector2.zero;
        if (animator) animator.SetFloat("Speed", 0);
        // Trigger Game over Scence and allow for restart (roguelike means ur frickin dead!) -- could add revive items here too
    }

    private void HandleHealthChanged(int current, int max)
    {
        // Add UI health bar, show damage on the sprite (redden it), etc
        healthBar.fillAmount = current / 100f;
    }
}
