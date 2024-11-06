using ModularData;
using PlasticGui.WorkspaceWindow.Items;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SharedReference<>), true)]
public class SharedReferenceDrawer : PropertyDrawer
{
    SerializedProperty rootProperty;
    VisualElement container;
    PropertyField valueField;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        rootProperty = property;

        SetHorizontalContainer();
        SerializedProperty isConstantProperty = rootProperty.FindPropertyRelative("IsConstant");
        PropertyField isConstantField = SetupConstantField(isConstantProperty);
        SetupValueField(isConstantProperty.boolValue);

        isConstantField.label = property.displayName;
        container.Add(isConstantField);
        container.Add(valueField);
        root.Add(container);
        return root;
    }

    public void SetHorizontalContainer()
    {
        container = new VisualElement();

        container.style.flexDirection = FlexDirection.Row;
    }

    public PropertyField SetupConstantField(SerializedProperty isConstantProperty)
    {
        PropertyField isConstantField = new PropertyField();
        isConstantField.bindingPath = isConstantProperty.propertyPath;
        isConstantField.RegisterValueChangeCallback(OnConstantTypeChanged);
        isConstantField.tooltip = "Toggle sets whether to use a constant or a shared reference";

        return isConstantField;
    }

    public SerializedProperty SetupValueField(bool isConstant) 
    {
        valueField = new PropertyField();
        valueField.style.flexGrow = 1;
        return SetValueField(isConstant);
    }

    public SerializedProperty SetValueField(bool isConstant)
    {
        string propertyRelativeName = isConstant ? "ConstantValue" : "SharedValue";
        SerializedProperty selectedProperty = rootProperty.FindPropertyRelative(propertyRelativeName);
        valueField.bindingPath = selectedProperty.propertyPath;
        return selectedProperty;
    }

    public void OnConstantTypeChanged(SerializedPropertyChangeEvent eventCallback)
    {
        if (valueField == null)
        {
            return;
        }

        valueField.RemoveFromHierarchy();
        SetupValueField(eventCallback.changedProperty.boolValue);
        valueField.Bind(eventCallback.changedProperty.serializedObject);
        container.Add(valueField);
    }

}
