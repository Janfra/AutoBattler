using System;
using UnityEngine;

namespace ModularData
{
    public class DynamicReferenceTypeConstraintAttribute : PropertyAttribute
    {
        public Type constraintType;
        public bool isSceneObjectsOnly;

        public DynamicReferenceTypeConstraintAttribute(Type constraintType, bool isSceneObjectsOnly = false)
        {
            if (!typeof(DynamicReferenceType).IsAssignableFrom(constraintType))
            {
                Debug.LogError("Dynamic Reference Type Constraint Attribute must be of DynamicReferenceType type or its children");
                return;
            }

            this.constraintType = constraintType;
            this.isSceneObjectsOnly = isSceneObjectsOnly;
        }
    }
}
