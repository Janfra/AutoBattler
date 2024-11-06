using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Float Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Float")]
    public class FloatReferenceType : BlackboardReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(SharedFloat);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is SharedFloat;
        }
    }
}
