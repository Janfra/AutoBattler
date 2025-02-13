using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Battle Unit Data Reference", menuName = "ScriptableObjects/DynamicReferenceType/Battle Unit Data")]
    public class BattleUnitDataReferenceType : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(SharedBattleUnitData);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is SharedBattleUnitData;
        }
    }
}

