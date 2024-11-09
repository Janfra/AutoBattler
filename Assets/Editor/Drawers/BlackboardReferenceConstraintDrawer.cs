using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(BlackboardReferenceConstraintAttribute))]
public class BlackboardReferenceConstraintDrawer : PropertyDrawer
{
    BlackboardReferenceDataDrawer drawer = new BlackboardReferenceDataDrawer();
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        BlackboardReferenceConstraintAttribute constraintAttribute = attribute as BlackboardReferenceConstraintAttribute;
        BlackboardReferenceType constraint = ScriptableObject.CreateInstance<BlackboardReferenceType>();
        if (constraint == null)
        {
            throw new NullReferenceException("Constraint given to the Blackboard Reference Constraint Attribute is not a Blackboard Reference Type - BlackboardReferenceConstraintDrawer.CreatePropertyGUI");
        }
        property.FindPropertyRelative(BlackboardReferenceDataDrawer.constraintPropertyName).objectReferenceValue = constraint;
        property.serializedObject.ApplyModifiedPropertiesWithoutUndo();

        VisualElement root = drawer.CreatePropertyGUI(property);
        if (root == null)
        {
            return root;
        }

        root.Q<PropertyField>(BlackboardReferenceDataDrawer.constraintFieldName).SetEnabled(false);
        ObjectField objectField = root.Q<ObjectField>(BlackboardReferenceDataDrawer.objectFieldName);
        if(objectField != null)
        {
            objectField.allowSceneObjects = constraintAttribute.isSceneObjectsOnly;
            objectField.objectType = typeof(MonoBehaviour);
        }

        return root;
    }
}
