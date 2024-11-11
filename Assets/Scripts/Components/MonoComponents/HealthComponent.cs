using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IAttackable
{
    [SerializeField]
    private IntReference maxHealth;
    [SerializeField]
    private IntReference health;

    private void Awake()
    {
        health.Value = maxHealth.Value;
    }

    public bool IsAttackable => health > 0;
    public void ReceiveAttack(AttackData attackData)
    {
        health.Value -= attackData.damage;
    }
}
