using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BattleUnitData
{
    public UnitDefinition unitData;
    public Transform transform;
    public IAttackable attackable;

    public BattleUnitData(Transform unitPosition, IAttackable attackable, UnitDefinition unitData)
    {
        this.unitData = unitData;
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
