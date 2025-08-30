using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public MonoBehaviour[] attacks;
    public Transform target;

    void Update()
    {
        // Basic Attacks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            (attacks[0] as IAttack)?.performAttack(target);
        }

        // Special Attacks
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            (attacks[1] as IAttack)?.performAttack(target);
        }

        
    }
}
