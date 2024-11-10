using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Movement Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Movement")]
    public class MovementReferenceType : BlackboardReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(Movement);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is Movement;
        }
    }
}
