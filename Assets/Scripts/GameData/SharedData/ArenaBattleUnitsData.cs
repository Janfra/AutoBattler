using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BattleUnitData
{
    public UnitData unitData;
    public Transform transform;

    public BattleUnitData(Transform unitPosition, UnitData unitData)
    {
        this.unitData = unitData;
        transform = unitPosition;    
    }
}

[CreateAssetMenu(fileName = "New Arena Units Data", menuName = "ScriptableObjects/SharedValues/Arena Units Data")]
public class ArenaBattleUnitsData : SharedList<BattleUnitData>
{
}
