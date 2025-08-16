using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float StartHealth;

    protected float Health { get; private set; }
    protected bool IsDead;

    public event Action OnDeath;

    protected virtual void Start()
    {
        Health = StartHealth;
    }

    public virtual void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health > 0 || IsDead)
        {
            return;
        }
        IsDead = true;
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
