using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemies/Attack/SelfDestructCharge")]
public class SelfDestructChargeSO : AttackBehaviorSO
{
    public float windup = 0.35f;
    public float chargeSpeed = 14f;
    public float chargeTime = 0.25f;
    public float explosionRadius = 2.2f;
    public int damage = 30;

    public float maxChargeTime = 0.2f; // cap on duration
    public float arriveEpsilon = 0.05f; // how close is arrived
    public float explodeDelay = 0.15f; // Wait before applying damage

    public string explodeTrigger = "Explode";
    public float explosionScale = 0.7f;


    public LayerMask damageMask;
    public GameObject vfx;

    public override AttackBehavior Create() => new Impl(this);

    public class Impl : AttackBehavior
    {
        private readonly SelfDestructChargeSO so;
        private Transform self;
        private System.Func<Transform> getTarget;
        private Rigidbody2D rb;

        public Impl(SelfDestructChargeSO so)
        {
            this.so = so;
        }

        private Animator animator;
        private Transform gfx;

        public override void Init(Transform self, Func<Transform> targetGetter)
        {
            this.self = self;
            this.getTarget = targetGetter;
            rb = self.GetComponent<Rigidbody2D>();
            animator = self.GetComponent<Animator>();
            var sr = self.GetComponent<SpriteRenderer>();
            if (sr) gfx = sr.transform;
        }

        public override IEnumerator Execute(Vector2 lockedPoint, System.Action onFinished, System.Action onExplodeSelf)
        {
            // 1) telegraph
            if (rb) rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(so.windup);

            // 2) rush to the locked point (physics step); no re-aiming
            float t = 0f;
            while (t < so.maxChargeTime)
            {
                t += Time.fixedDeltaTime;

                Vector2 pos = rb.position;
                Vector2 to = lockedPoint - pos;
                float dist = to.magnitude;

                // arrived or would overshoot → snap & stop
                float step = so.chargeSpeed * Time.fixedDeltaTime;
                if (dist <= so.arriveEpsilon || step >= dist)
                {
                    if (rb) { rb.linearVelocity = Vector2.zero; rb.MovePosition(lockedPoint); }
                    break;
                }

                // keep dashing
                if (rb) rb.linearVelocity = (to / dist) * so.chargeSpeed;

                yield return new WaitForFixedUpdate();
            }

            // 3) freeze at impact point (don’t let anything move it)
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.MovePosition(lockedPoint);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;   // <- key line to avoid drift
            }

            // 4) (optional) tiny pause to sync with animation; damage happens AFTER
            if (gfx && so.explosionScale > 0f) gfx.localScale = Vector3.one * so.explosionScale;
            if (animator && !string.IsNullOrEmpty(so.explodeTrigger)) animator.SetTrigger(so.explodeTrigger);
            if (so.explodeDelay > 0f)
                yield return new WaitForSeconds(so.explodeDelay);


            // 5) apply damage once at the explosion point
            var hits = Physics2D.OverlapCircleAll(self.position, so.explosionRadius, so.damageMask);
            foreach (var h in hits)
            {
                var hp = h.GetComponent<Health>();
                if (hp) hp.TakeDamage(so.damage);
            }

            // 6) destroy enemy; DO NOT call onFinished anywhere in this attack
            onExplodeSelf?.Invoke();
        }
    }
}


