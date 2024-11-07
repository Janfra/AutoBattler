using MyCustomEditor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using GameAI;

[CustomEditor(typeof(Blackboard), true)]
public class BlackboardEditor : CustomUIElementsEditor<Blackboard>
{
    Dictionary<IEventHandler, int> fieldToNumber;
    List<VisualElement> objectFields;

    public override void OnEditorEnabled()
    {
        base.OnEditorEnabled();
        fieldToNumber = new Dictionary<IEventHandler, int>();
        objectFields = new List<VisualElement>();
    }

    public override void OnTreeCloned(VisualElement root)
    {
        UpdateInspectorOnEdit(root);
        CreateObjectFields(root);
    }

    public void UpdateInspectorOnEdit(VisualElement root)
    {
        PropertyField constraintField = root.Q<PropertyField>("Constraints");
        constraintField.RegisterValueChangeCallback(OnConstraintsUpdated);
    }

    public void CreateObjectFields(VisualElement root)
    {
        BoardReferenceData[] referencesConstraints = editorTarget.GetDataContainersCopy();
        if (referencesConstraints == null || referencesConstraints.Length <= 0)
        {
            return;
        }

        int index = 0;
        foreach (BoardReferenceData constraint in referencesConstraints)
        {
            if (!constraint.HasValidConstraint())
            {
                continue;
            }

            ObjectField objectField = new ObjectField();
            Type constraintType = constraint.GetExpectedType();
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
            objectField.RegisterValueChangedCallback(SetObjectReference);
            objectField.value = constraint.GetReference();
            objectField.Bind(serializedObject);
            root.Add(objectField);
            objectFields.Add(objectField);
            fieldToNumber[objectField] = index;
            index++;
        }
    }

    public void OnConstraintsUpdated(SerializedPropertyChangeEvent callback)
    {
        foreach (VisualElement field in objectFields)
        {
            field.RemoveFromHierarchy();
        }
        fieldToNumber.Clear();
        CreateObjectFields(editorRoot);
    }

    public void SetObjectReference(ChangeEvent<Object> eventData)
    {
        int index = fieldToNumber[eventData.currentTarget];
        if (!editorTarget.SetReferenceAt(index, eventData.newValue))
        {
            ObjectField field = (ObjectField)eventData.currentTarget;
            if (field != null)
            {
                field.value = null;
            }
        }
    }
}