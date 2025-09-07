using UnityEngine;

public class Scratch : MonoBehaviour, IAttack
{
    public float damage = 20f;
    public float range = 1f; // Distance in front of animal
    public LayerMask enemyLayer; // Layer mask to identify enemies

    public void performAttack(Transform target)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.position, range, enemyLayer); // Attack Radius

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);

        }

        
    }
}