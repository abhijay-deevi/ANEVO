using UnityEngine;

public class Fireball : MonoBehaviour, IAttack
{
    public GameObject fireballPrefab;
    public float speed = 10f;

    public void performAttack(Transform target)
    {
        
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Vector2 dir = (target.position - transform.position).normalized;
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * speed;
    }
}
