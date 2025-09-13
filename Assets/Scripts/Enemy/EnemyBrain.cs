using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyDefinition definition;
    [SerializeField] private string playerTag = "player";
    [SerializeField] private LayerMask damageMask;

    private Rigidbody2D rb;
    private Transform self;
    private Transform target;
    private float health;

    private MovementBehavior movement;
    private AttackBehavior attack;
    private bool isAttacking;

    // Setting up all the variables
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        self = transform;
        health = definition.maxHealth;

        movement = definition.movement.Create();
        attack = definition.attack.Create();

        System.Func<Transform> targetGetter = () => target;
        movement.Init(rb, self, targetGetter);
        attack.Init(self, targetGetter);
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player) target = player.transform;
    }

    private void Update()
    {
        if (!target || isAttacking) return;

        if (movement.ShouldAttack(out var aimDir))
        {
            StartCoroutine(RunAttack(aimDir));
        }

        if (movement.ShouldAttack(out _))
        {
            Vector2 lockedPoint = target.position;
            StartCoroutine(RunAttack(lockedPoint));
        }
    }

    private void FixedUpdate()
    {
        if (!target || isAttacking) return;
        movement.Tick(Time.fixedDeltaTime);
    }

    private IEnumerator RunAttack(Vector2 aimDir)
    {
        isAttacking = true;

        void Finished() => isAttacking = false;
        void ExplodeMe() => Die(true);

        yield return attack.Execute(aimDir, Finished, ExplodeMe);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f) Die(false);
    }

    private void Die(bool fromAttack)
    {
        //if (!definition.deathVFX) Instantiate(definition.deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
