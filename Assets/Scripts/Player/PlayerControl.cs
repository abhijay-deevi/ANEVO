using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
    
{

    public float speed = 5;
    public float redtime = 0.1f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement; // stores x, y

    // Health Stuff
    private Health health;
    public Image healthBar;
    private float prevHealth = -5f;

    // Where the melee attacks should hit
    public Transform triangle;
    public GameObject frontattack;
    public Vector3 frontalAttackOffset = new Vector3(5, 0, 0);
    private SpriteRenderer fas;

    // Flashing Color Effects
    private SpriteRenderer sr;
    private Color redden = Color.red;
    private Color greenden = Color.green;
    private Color originalColor = Color.white;


    // Basically our constructor
    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        if (frontattack) fas = frontattack.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
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

    // Movement
    private void Update()
    {
        if (health.isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // performance trick

        // Switches the position of our attack circle
        if (movement.x > 0)
        {
            triangle.localPosition = new Vector2(5, 0f);
            frontattack.transform.localPosition = frontalAttackOffset;
            if (fas) fas.flipX = false;
        }
        else if (movement.x < 0)
        {
            triangle.localPosition = new Vector2(-5, 0f);
            frontattack.transform.localPosition = new Vector3(-frontalAttackOffset.x, frontalAttackOffset.y, frontalAttackOffset.z);
            if (fas) fas.flipX = true;
        }
    }

    // Movement
    private void FixedUpdate()
    {
        if (health.isDead) return;
        // rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed);
    }

    private void HandleDeath()
    {
        movement = Vector2.zero;
        if (animator) animator.SetFloat("Speed", 0);
        // Trigger Game over Scence and allow for restart (roguelike means ur dead!) -- could add revive items here too
    }

    private void HandleHealthChanged(float current, float max)
    {
        // Add UI health bar, show damage on the sprite (redden it), etc
        healthBar.fillAmount = current / max;

        if (prevHealth == -5)
        {
            prevHealth = current;
            return;
        }
        
        if (prevHealth > current)
        {
            StartCoroutine(Flash(redden));
        } else if (prevHealth < current)
        {
            StartCoroutine(Flash(greenden));
        }
        
        prevHealth = current;
                
    }

    private void OnDrawGizmos()
    {
        Scratch scratch = GetComponent<Scratch>();
        float range = scratch.range;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private IEnumerator Flash(Color flashColor)
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(redtime);
        sr.color = originalColor;
    }
}
