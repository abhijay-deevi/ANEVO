using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int maxHP = 100;
    [SerializeField] private float invulnSeconds = 0.5f;
    [SerializeField] private bool destroyOnDeath = false;

    private int hp;
    private bool invulnerable;
    private bool dead;

    public event Action<int, int> OnHealthChanged; // Observer pattern 
    public event Action OnDied;

    public int Current => hp;
    public int Max => maxHP;
    public bool isDead => dead;

    private void Awake()
    {
        hp = maxHP;
        OnHealthChanged?.Invoke(hp, maxHP);
    }

    public void TakeDamage(int amount)
    {
        if (dead || invulnerable || amount <= 0)
        {
            return;
        }

        hp = Mathf.Max(0, hp - amount);
        OnHealthChanged?.Invoke(hp, maxHP);

        if (hp == 0)
        {
            dead = true;
            OnDied?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
            return;
        }

        if (invulnSeconds > 0f) StartCoroutine(Invuln());

    }

    public IEnumerator Invuln()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnSeconds);
        invulnerable = false;
    }

    public void Heal(int amount)
    {
        if (dead || amount <= 0) return;

        hp = Mathf.Min(maxHP, hp + amount);
        OnHealthChanged?.Invoke(hp, maxHP);
    }
}
