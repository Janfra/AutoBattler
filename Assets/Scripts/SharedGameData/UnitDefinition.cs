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
    private Sprite sprite;
    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private SharedFloat maxSpeed;
    public SharedFloat MaxSpeed { get => maxSpeed; }

    [SerializeField]
    private SharedInt maxHealth;
    public SharedInt MaxHealth { get => maxHealth; }

    [SerializeField]
    private SharedInt damage;
    public SharedInt Damage { get => damage; }

    [SerializeField]
    private SharedFloat attackSpeed;
    public SharedFloat AttackSpeed { get => attackSpeed; }
}
