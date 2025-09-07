using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float damage;

    public LayerMask enemyLayer;
    public GameObject destroyEffect;

    private Vector2 moveDirection;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    // Must be PUBLIC so Fireball.cs can call it
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Update()
    {
        // Move strictly in the assigned direction
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Optional: collision check
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, moveDirection, 0.1f, enemyLayer);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hitInfo.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
