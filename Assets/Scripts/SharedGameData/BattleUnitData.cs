using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleUnitData
{
    public UnitDefinition unitDefinition;
    public Transform transform;
    public IAttackable attackable;

    public BattleUnitData(Transform unitPosition, IAttackable attackable, UnitDefinition unitDefinition)
    {
        this.unitDefinition = unitDefinition;
        this.attackable = attackable;
        transform = unitPosition;
    }

    public bool IsValid()
    {
        return transform != null && attackable != null;
    }

    public bool HasTransform()
    {
        return transform != null;
    }

    public bool IsAttackable()
    {
        return attackable != null && attackable.IsAttackable;
    }
}
