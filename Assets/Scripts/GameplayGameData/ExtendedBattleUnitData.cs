using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExtendedBattleUnitData : BattleUnitData
{
    public SharedGraphNodeHandle unitPathfindHandle;

    public ExtendedBattleUnitData(Transform unitPosition, IAttackable attackable, UnitDefinition unitData, GraphNodeHandle handle) : base(unitPosition, attackable, unitData)
    {
        SharedGraphNodeHandle sharedHandle = ScriptableObject.CreateInstance<SharedGraphNodeHandle>();
        sharedHandle.Value = handle;
        unitPathfindHandle = sharedHandle;
    }
}
