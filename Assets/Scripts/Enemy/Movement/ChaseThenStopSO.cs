using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemies/Movement/ChaseThenStop")]
public class ChaseThenStopSO : MovementBehaviorSO
{
    public float moveSpeed = 3.5f;
    public float stopDistance = 1.6f;

    public override MovementBehavior Create() => new Impl(moveSpeed, stopDistance);

    private class Impl : MovementBehavior
    {
        private readonly float speed, stopDist;
        private Rigidbody2D rb;
        private Transform self;
        private System.Func<Transform> getTarget;

        public Impl(float speed, float stopDist)
        {
            this.speed = speed;
            this.stopDist = stopDist;
        }

        public override void Init(Rigidbody2D rb, Transform self, Func<Transform> targetGetter)
        {
            this.rb = rb;
            this.self = self;
            this.getTarget = targetGetter;
        }

        public override void Tick(float dt)
        {
            var t = getTarget();
            if (t != null)
            {
                return;
            }
            var to = (Vector2)(t.position - self.position);
            var dist = to.magnitude;
            if (dist > stopDist)
            {
                rb.MovePosition(rb.position + to.normalized * speed * dt);
            } else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        public override bool ShouldAttack(out Vector2 aimDir)
        {
            var t = getTarget();
            if (t == null)
            {
                aimDir = Vector2.zero;
                return false;
            }

            aimDir = ((Vector2)(t.position - self.position)).normalized;
            var dist = Vector2.Distance(self.position, t.position);
            return dist <= stopDist + 0.01f;
        }
    }
}
