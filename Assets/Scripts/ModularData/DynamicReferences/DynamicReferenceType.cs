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

    public abstract class DynamicReferenceTypeTemplate<T> : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(T);
        }

        public override bool IsObjectValid(Object checkObject)
        {
            return checkObject is T;
        }
    }

    public abstract class DynamicInterfaceReferenceType<Interface> : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(Interface);
        }

        public override bool IsObjectValid(Object checkObject)
        {
            return IsObjectOfInterfaceType(checkObject);
        }

        public bool IsObjectOfInterfaceType(Object checkObject)
        {
            return checkObject is Interface && typeof(Interface).IsAssignableFrom(checkObject.GetType());
        }
    }
}
