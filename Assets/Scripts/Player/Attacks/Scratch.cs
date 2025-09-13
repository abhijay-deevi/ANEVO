using UnityEditor.PackageManager;
using UnityEngine;

public class Scratch : MonoBehaviour, IAttack
{
    public int damage = 20;
    public float range = 1f; // Distance in front of animal
    public LayerMask enemyLayer; // Layer mask to identify enemies

    public void performAttack(Transform target)
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.position, range, enemyLayer); // Attack Radius

        foreach (Collider2D e in hitEnemies)
        {
            var enemy = e.GetComponent<Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

        }

        
    }
}