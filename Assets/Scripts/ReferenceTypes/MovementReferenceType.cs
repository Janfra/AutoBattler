using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Movement Reference", menuName = "ScriptableObjects/DynamicReferenceType/Movement")]
    public class MovementReferenceType : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(MovementComponent);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is MovementComponent;
        }
    }
}
