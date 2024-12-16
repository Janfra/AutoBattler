using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModularData
{
    [Serializable]
    public struct DynamicReference
    {
        [SerializeField]
        private DynamicReferenceType referenceType;

        [ReadOnly]
        [SerializeField]
        private Object objectReference;

        public DynamicReference(DynamicReferenceType referenceType, Object objectReference)
        {
            this.referenceType = referenceType;
            this.objectReference = null;
            SetReference(objectReference);
        }

        public Object GetReference()
        {
            return objectReference;
        }

        public bool HasValidConstraint()
        {
            return referenceType != null;
        }

        public bool HasValidReference()
        {
            return objectReference != null;
        }

        public bool SetReference(Object reference)
        {
            if (reference == null)
            {
                return false;
            }

            if (!referenceType.IsObjectValid(reference))
            {
                return false;
            }

            objectReference = reference;
            return true;
        }

        public Type GetExpectedType()
        {
            if (!HasValidConstraint())
            {
                return null;
            }

            return referenceType.GetDataObjectType();
        }

        public DynamicReferenceType GetReferenceTypeConstraint()
        {
            return referenceType;
        }

        public void ValidateReference()
        {
            if (!referenceType || !referenceType.IsObjectValid(objectReference))
            {
                objectReference = null;
            }
        }

        public DynamicReference GetWithUniqueConstraint()
        {
            if (referenceType == null)
            {
                return new DynamicReference();
            }

            referenceType = ScriptableObject.CreateInstance<DynamicReferenceType>();
            return this;
        }
    }
}
