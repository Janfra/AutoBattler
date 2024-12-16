using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New BlackboardDE Target Reference", menuName = "ScriptableObjects/DynamicReferenceType/BlackboardDE Target")]
    public class BlackboardDETargetReferenceType : DynamicInterfaceReferenceType<IBlackboardDITarget>
    {
        public override Type GetDataObjectType()
        {
            return typeof(IBlackboardDITarget);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return IsObjectOfInterfaceType(checkObject);
        }
    }
}
