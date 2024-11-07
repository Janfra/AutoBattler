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
        if (!typeof(BlackboardReferenceType).IsAssignableFrom(constraintType))
        {
            Debug.LogError("Blackboard Reference Constraint Attribute must be of BlackboardReferenceType type or its children");
            return;
        }

        this.constraintType = constraintType;
        this.isSceneObjectsOnly = isSceneObjectsOnly;
    }
}
