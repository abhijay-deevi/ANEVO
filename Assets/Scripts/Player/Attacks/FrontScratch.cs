using System.Drawing;
using UnityEngine;

public class FrontScratch : MonoBehaviour, IAttack
{
    public int damage = 30;
    public float range = 1f; // Distance in front of animal
    public LayerMask enemyLayer; // Layer mask to identify enemies

    public int size = 10;

    public Collider2D attackCollider;

    public void performAttack(Transform target)
    {

        attackCollider = target.Find("Circle").GetComponent<Collider2D>();

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;

        Collider2D[] results = new Collider2D[size];
        int hitCount = attackCollider.Overlap(filter, results);

        for (int i = 0; i < hitCount; i++)
        {
            var enemy = results[i].GetComponent<Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

    }
}