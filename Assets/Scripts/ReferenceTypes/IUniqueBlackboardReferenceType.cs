using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/DynamicReferenceType/Unique Blackboard Referencer", order = 1)]
    public class IUniqueBlackboardReferenceType : DynamicInterfaceReferenceType<IUniqueBlackboardReferencer>
    {
        public override Type GetDataObjectType()
        {
            return typeof(IUniqueBlackboardReferencer);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return IsObjectOfInterfaceType(checkObject);
        }
    }
}

