using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Arena Data Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Arena Data")]
    public class ArenaDataReferenceType : BlackboardReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(ArenaData);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is ArenaData;
        }
    }
}
