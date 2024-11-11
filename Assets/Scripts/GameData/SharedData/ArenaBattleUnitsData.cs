using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BattleUnitData
{
    public UnitDefinition unitData;
    public Transform transform;

    public BattleUnitData(Transform unitPosition, UnitDefinition unitData)
    {
        this.unitData = unitData;
        transform = unitPosition;    
    }

    public bool IsValid()
    {
        return transform != null;
    }
}

[CreateAssetMenu(fileName = "New Arena Units Data", menuName = "ScriptableObjects/SharedValues/Arena Units Data")]
public class ArenaBattleUnitsData : SharedList<BattleUnitData>
{
}
