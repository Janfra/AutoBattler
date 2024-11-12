using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    public int damage;
}

public interface IAttackable
{
    public delegate void OnDestroy();
    public event OnDestroy Destroyed;

    // Potentially change it to pass it data to decide instead, but for now keep it simple
    public bool IsAttackable { get; }
    public void ReceiveAttack(AttackData attackData);
}
