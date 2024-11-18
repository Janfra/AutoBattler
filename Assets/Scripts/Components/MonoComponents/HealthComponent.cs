using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IAttackable
{
    public event IAttackable.OnDestroy Destroyed;

    [SerializeField]
    private IntReference maxHealth;
    [SerializeField]
    private IntReference health;

    private void Awake()
    {
        if (maxHealth == null)
        {
            maxHealth = new IntReference();
        }

        if (health == null)
        {
            health = new IntReference();
        }

        health.Value = maxHealth.Value;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth.Value = newMaxHealth;
        health.Value = newMaxHealth;
    }

    public bool IsAttackable => health > 0;
    public void ReceiveAttack(AttackData attackData)
    {
        health.Value -= attackData.damage;
        if (health.Value <= 0)
        {
            Destroyed?.Invoke();
            Destroyed = null;
            gameObject.SetActive(false);
        }
    }
}
