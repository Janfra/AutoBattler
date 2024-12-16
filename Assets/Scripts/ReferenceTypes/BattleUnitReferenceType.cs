using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Battle Unit Reference", menuName = "ScriptableObjects/DynamicReferenceType/Battle Unit")]
    public class BattleUnitReferenceType : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(BattleUnit);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is BattleUnit;
        }
    }
}
