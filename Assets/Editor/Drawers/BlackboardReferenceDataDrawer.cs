using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(BoardReferenceData))]
public class BlackboardReferenceDataDrawer : PropertyDrawer
{
    const string objectFieldName = "ObjectField";

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        BoardReferenceData data = (BoardReferenceData)property.boxedValue;

        PropertyField field = new PropertyField();
        field.BindProperty(property.FindPropertyRelative("constraint"));
        root.Add(field);
        field.RegisterValueChangeCallback(OnReferenceTypeChanged);

        return root;
    }

    public void OnReferenceTypeChanged(SerializedPropertyChangeEvent eventData)
    {
        string[] propertyPaths = eventData.changedProperty.propertyPath.Split(".");
        string propertyPath = "";
        for (int i = 0; i < propertyPaths.Length - 1; i++)
        {
            propertyPath += propertyPaths[i];
            if (i < propertyPaths.Length - 2)
            {
                propertyPath += ".";
            }
        }

        SerializedProperty property = eventData.changedProperty.serializedObject.FindProperty(propertyPath);
        if(property == null)
        {
            return;
        }

        VisualElement root = ((VisualElement)eventData.currentTarget).parent;
        ObjectField objectField = root.Q<ObjectField>(objectFieldName);
        if(objectField != null)
        {
            BoardReferenceData data = (BoardReferenceData)property.boxedValue;
            if (!data.HasValidConstraint() || objectField.objectType != data.GetExpectedType())
            {
                objectField.RemoveFromHierarchy();
                CreateObjectField(property, root);
            }
        }
        else
        {
            CreateObjectField(property, root);
        }
    }

    public void CreateObjectField(SerializedProperty property, VisualElement root)
    {
        BoardReferenceData data = (BoardReferenceData)property.boxedValue;
        if (!data.HasValidConstraint())
        {
            return;
        }

        ObjectField objectField = new ObjectField();
        Type constraintType = data.GetExpectedType();
        if (constraintType.IsInterface)
        {
            objectField.objectType = typeof(Object);
        }
        else
        {
            objectField.objectType = constraintType;
        }

        objectField.label = constraintType.Name;
        objectField.allowSceneObjects = true;
        objectField.value = data.GetReference();
        objectField.name = objectFieldName;
        objectField.bindingPath = property.FindPropertyRelative("objectReference").propertyPath;
        objectField.Bind(property.serializedObject);

        root.Add(objectField);
    }
}
