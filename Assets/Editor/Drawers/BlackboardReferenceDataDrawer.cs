using GameAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(BoardReferenceData))]
public class BlackboardReferenceDataDrawer : PropertyDrawer
{
    public const string constraintFieldName = "ConstraintField";
    public const string objectFieldName = "ObjectField";
    public const string constraintPropertyName = "constraint";
    public const string objectReferencePropertyName = "objectReference";
    Dictionary<IEventHandler, SerializedProperty> fieldToProperty = new Dictionary<IEventHandler, SerializedProperty>();

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();

        PropertyField field = new PropertyField();
        field.BindProperty(property.FindPropertyRelative(constraintPropertyName));
        field.name = constraintFieldName;
        root.Add(field);
        field.RegisterValueChangeCallback(OnReferenceTypeChanged);

        return root;
    }

    public void OnReferenceTypeChanged(SerializedPropertyChangeEvent eventData)
    {
        string propertyPath = GetParentPropertyPath(eventData.changedProperty);
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

        ObjectField objectField = GetObjectFieldForType(data.GetExpectedType(), property);
        fieldToProperty[objectField] = property;

        root.Add(objectField);
    }

    public ObjectField GetObjectFieldForType(Type constraintType, SerializedProperty property) 
    {
        ObjectField objectField = new ObjectField();
        if (constraintType.IsInterface)
        {
            objectField.objectType = typeof(Object);
        }
        else
        {
            objectField.objectType = constraintType;
        }

        SerializedProperty objectReferenceProperty = property.FindPropertyRelative(objectReferencePropertyName);
        objectField.label = constraintType.Name;
        objectField.allowSceneObjects = true;
        objectField.name = objectFieldName;
        objectField.RegisterValueChangedCallback(SetObjectReference);
        objectField.bindingPath = objectReferenceProperty.propertyPath;
        objectField.Bind(property.serializedObject);

        return objectField;
    }

    public string GetParentPropertyPath(SerializedProperty property)
    {
        string[] propertyPaths = property.propertyPath.Split(".");
        string propertyPath = "";
        for (int i = 0; i < propertyPaths.Length - 1; i++)
        {
            propertyPath += propertyPaths[i];
            if (i < propertyPaths.Length - 2)
            {
                propertyPath += ".";
            }
        }

        return propertyPath;
    }

    public void SetObjectReference(ChangeEvent<Object> eventData)
    {
        ObjectField field = eventData.currentTarget as ObjectField;
        if (field == null)
        {
            return;
        }

        SerializedProperty property = fieldToProperty[field];
        if (property == null)
        {
            return;
        }

        BoardReferenceData data = (BoardReferenceData)property.boxedValue;
        if (eventData.newValue != null && !data.SetReference(eventData.newValue))
        {
            Debug.LogError($"Attempted to set invalid value to blackboard reference - Expected: {data.GetExpectedType().Name} Received: {eventData.newValue.GetType().Name}");
            field.value = null;
        }
    }
}
