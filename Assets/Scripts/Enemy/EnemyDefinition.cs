using UnityEngine;


public abstract class MovementBehaviorSO : ScriptableObject
{
    public abstract MovementBehavior Create();
}

public abstract class AttackBehaviorSO : ScriptableObject
{
    public abstract AttackBehavior Create();
}

[CreateAssetMenu (menuName = "Enemies/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    [Header("Stats")]
    public float maxHealth = 50f;

    [Header("Behaviors")]
    public MovementBehaviorSO movement;
    public AttackBehaviorSO attack;

    //[Header("VFX")]
    //public GameObject deathVFX;
    //public GameObject attackVFX;
    //public AudioClip attackSFX;

}
