using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModularData
{
    public abstract class DynamicReferenceType : ScriptableObject
    {
        public abstract Type GetDataObjectType();
        public abstract bool IsObjectValid(Object checkObject);
    }

    public abstract class DynamicInterfaceReferenceType<Interface> : DynamicReferenceType
    {
        public bool IsObjectOfInterfaceType(Object checkObject)
        {
            return checkObject is Interface && typeof(Interface).IsAssignableFrom(checkObject.GetType());
        }
    }
}
