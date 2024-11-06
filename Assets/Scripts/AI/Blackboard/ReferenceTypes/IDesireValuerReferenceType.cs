using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/BlackboardReferenceType/DesireValuer", order = 2)]
    public class IDesireValuerReferenceType : BlackboardInterfaceReferenceType<IDesireValuer>
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
