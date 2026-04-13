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

    [SerializeField] private float health;

    public event Action OnDeath;
    private bool isDead;
    public void TakeDamage(float damage)
    {
        if(isDead) return;

        health -= damage;
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