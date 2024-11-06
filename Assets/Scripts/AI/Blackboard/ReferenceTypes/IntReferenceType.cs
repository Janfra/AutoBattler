using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Int Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Int")]
    public class IntReferenceType : BlackboardReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(SharedInt);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is SharedInt;
        }
    }
}
