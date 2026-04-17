using System;
using UnityEngine;

public enum Team
{
    Player,
    Enemy
}

public class HealthController : MonoBehaviour
{
    [SerializeField] private Team team;
    public Team Team => team;

    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public float Health => health;
    public float MaxHealth => maxHealth;

    public event Action<float,float> OnHealthChange;
    public event Action OnDeath;
    private bool isDead;
    public void TakeDamage(float damage)
    {
        if(isDead) return;
        
        health -= damage;
        health= Mathf.Clamp(health, 0, maxHealth);

        OnHealthChange?.Invoke(health, MaxHealth);

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}