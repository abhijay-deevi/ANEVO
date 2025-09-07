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

        public override void Init(Transform self, Func<Transform> targetGetter)
        {
            this.self = self;
            this.getTarget = targetGetter;
            rb = self.GetComponent<Rigidbody2D>();
        }

        public override IEnumerator Execute(Vector2 aimDir, Action onFinished, Action onExplodeSelf)
        {
            // Wind-up
            if (rb) rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(so.windup);

            // Charge
            if (rb) rb.linearVelocity = aimDir * so.chargeSpeed;
            float t = 0f;
            while (t < so.chargeTime)
            {
                t += Time.deltaTime;
                yield return null;
            }

            // Explode
            //if (so.vfx) Object.Instantiate(so.vfx, self.position, Quaternion.identity);
            var hits = Physics2D.OverlapCircleAll(self.position, so.explosionRadius, so.damageMask);
            foreach (var h in hits)
            {
                var hp = h.GetComponent<Health>();
                if (hp) hp.TakeDamage(so.damage);
            }
            Debug.Log("Exploreded at " + self.position);

            onExplodeSelf?.Invoke();
        }
    }
}


