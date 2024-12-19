using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefinition : ScriptableObject
{
    [SerializeField]
    private string unitName;
    public string UnitName { get => unitName; }

    [SerializeField]
    private RuntimeAnimatorController animatorController;
    public RuntimeAnimatorController AnimatorController { get => animatorController; }

    [SerializeField]
    private SharedFloat maxSpeed;
    public SharedFloat MaxSpeed { get => maxSpeed; }

    [SerializeField]
    private SharedInt maxHealth;
    public SharedInt MaxHealth { get => maxHealth; }

    [SerializeField]
    private SharedInt health;
    public SharedInt Health { get => health; }

    [SerializeField]
    private SharedInt damage;
    public SharedInt Damage { get => damage; }

    [SerializeField]
    private SharedFloat attackSpeed;
    public SharedFloat AttackSpeed { get => attackSpeed; }
}
