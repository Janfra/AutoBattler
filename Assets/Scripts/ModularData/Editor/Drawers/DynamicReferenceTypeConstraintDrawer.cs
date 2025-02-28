using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ModularData.Editor
{
    [CustomPropertyDrawer(typeof(DynamicReferenceTypeConstraintAttribute))]
    public class DynamicReferenceTypeConstraintDrawer : PropertyDrawer
    {
        DynamicReferenceDrawer drawer = new DynamicReferenceDrawer();
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            DynamicReferenceTypeConstraintAttribute constraintAttribute = attribute as DynamicReferenceTypeConstraintAttribute;
            DynamicReferenceType constraint = ScriptableObject.CreateInstance(constraintAttribute.constraintType.Name) as DynamicReferenceType;
            if (constraint == null)
            {
                throw new NullReferenceException("Constraint given to the Blackboard Reference Constraint Attribute is not a Blackboard Reference Type - BlackboardReferenceConstraintDrawer.CreatePropertyGUI");
            }
            property.FindPropertyRelative(DynamicReferenceDrawer.constraintPropertyName).objectReferenceValue = constraint;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();

            VisualElement root = drawer.CreatePropertyGUI(property);
            if (root == null)
            {
                return root;
            }

            root.Q<PropertyField>(DynamicReferenceDrawer.constraintFieldName).SetEnabled(false);
            ObjectField objectField = root.Q<ObjectField>(DynamicReferenceDrawer.objectFieldName);
            if(objectField != null)
            {
                objectField.allowSceneObjects = constraintAttribute.isSceneObjectsOnly;
                objectField.objectType = typeof(MonoBehaviour);
            }

            return root;
        }
    }
}
