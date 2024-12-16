using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardReferenceConstraintAttribute : PropertyAttribute
{
    public Type constraintType;
    public bool isSceneObjectsOnly;

    public BlackboardReferenceConstraintAttribute(Type constraintType, bool isSceneObjectsOnly = false)
    {
        if (!typeof(DynamicReferenceType).IsAssignableFrom(constraintType))
        {
            Debug.LogError("Blackboard Reference Constraint Attribute must be of DynamicReferenceType type or its children");
            return;
        }

        this.constraintType = constraintType;
        this.isSceneObjectsOnly = isSceneObjectsOnly;
    }
}
