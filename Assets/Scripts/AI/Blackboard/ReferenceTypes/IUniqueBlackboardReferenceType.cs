using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Unique Blackboard Referencer", order = 1)]
    public class IUniqueBlackboardReferenceType : BlackboardInterfaceReferenceType<IUniqueBlackboardReferencer>
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

