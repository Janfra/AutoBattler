using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/DynamicReferenceType/DesireValuer", order = 2)]
    public class IDesireValuerReferenceType : DynamicInterfaceReferenceType<IDesireValuer>
    {
        public override Type GetDataObjectType()
        {
            return typeof(IDesireValuer);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return IsObjectOfInterfaceType(checkObject);    
        }
    }
}
