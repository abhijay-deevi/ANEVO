using UnityEngine;

public class MeleeConstrict : MonoBehaviour, IAttack
{
    public float damage = 20f;
    public float range = 1f;

    public void performAttack(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) <= range)
        {
            Debug.Log("Attacking with Constrict!");
        }
        else
        {
            Debug.Log("Target is out of range for constrict attack.");
        }   
    }
}
