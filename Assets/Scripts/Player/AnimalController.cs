using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public MonoBehaviour[] attacks;
    public Transform target;

    private Animator meleeAnimator;
    private Animator frontalAnimator;

    private void Start()
    {
        meleeAnimator = transform.Find("Melee Attack").GetComponent<Animator>();
        frontalAnimator = transform.Find("Frontal Attack").GetComponent <Animator>();
    }

    void Update()
    {
        // Basic Attacks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            (attacks[0] as IAttack)?.performAttack(target);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) // Testing Attack to see what damage does.  
        {
            var player = GameObject.Find("Player");
            var hp = player.GetComponent<Health>();
            if (hp) hp.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) // Testing Attack to see what damage does.  
        {
            var player = GameObject.Find("Player");
            var hp = player.GetComponent<Health>();
            if (hp) hp.Heal(10);
        }

        // Special Attacks
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            (attacks[1] as IAttack)?.performAttack(target);
            meleeAnimator.SetTrigger("Circle Scratch");

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            (attacks[2] as IAttack)?.performAttack(target);
            frontalAnimator.SetTrigger("Forward Scratch");
        }

        
    }
}
