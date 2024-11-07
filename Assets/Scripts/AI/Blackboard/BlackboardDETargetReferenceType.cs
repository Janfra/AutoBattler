using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public class BlackboardDETargetReferenceType : BlackboardInterfaceReferenceType<IBlackboardDITarget>
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
