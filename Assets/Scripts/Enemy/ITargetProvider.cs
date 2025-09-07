using UnityEngine;

public interface ITargetProvider
{
    Transform GetTarget();
}

// Will allow us to have different movements for different enemies
public abstract class MovementBehavior
{
    // Once per enemy instance
    public virtual void Init(Rigidbody2D rb, Transform self, System.Func<Transform> targetGetter) { } // Virtual can be overriden by subclass
    // What it does while not attacking
    public abstract void Tick(float dt);
    // if enemy should attack or not
    public abstract bool ShouldAttack(out Vector2 aimDir);

}

// Will allow us to have different attack types (melee, ranged, etc.)
public abstract class AttackBehavior
{
    public virtual void Init(Transform self, System.Func<Transform> targetGetter) { } // System.Func<Transform> is a delegate variable
    public abstract System.Collections.IEnumerator Execute(Vector2 aimDir, System.Action onFinished, System.Action onExplodeSelf);
}
