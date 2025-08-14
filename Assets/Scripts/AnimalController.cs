using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public MonoBehaviour[] attacks;
    public Transform target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            (attacks[0] as IAttack)?.performAttack(target);
        }
    }
}
