using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    [SerializeField]
    protected Object targetObject;  
    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    
    protected void Draw(SerializedObject serializedObject, bool drawChildren)
    {
        if(serializedObject == null)
        {
            return;
        }

        currentProperty = serializedObject.GetIterator();
        if (currentProperty == null)
        {
            return;
        }

        if (currentProperty.depth == -1)
        {
            if (!currentProperty.Next(true))
            {
                return;
            }
        }

        SerializedProperty propertyCopy = currentProperty.Copy();
  
        if (propertyCopy.isArray)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            DrawSidebar(propertyCopy);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true));
            if (selectedProperty != null)
            {
                DrawProperties(propertyCopy, drawChildren);
            }
            else
            {
                EditorGUILayout.LabelField("Please select an item from the list");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            DrawProperties(propertyCopy, drawChildren);
        }
    }

    protected void DrawProperties(SerializedProperty propertyIterator, bool drawChildren)
    {
        string lastPropertyPath = string.Empty;
        if (propertyIterator.NextVisible(drawChildren))
        {
            do
            {
                if (propertyIterator == null)
                {
                    break;
                }

                if (propertyIterator.isArray && propertyIterator.propertyType == SerializedPropertyType.Generic)
                {
                    DrawFoldout(propertyIterator);
                    if (propertyIterator.isExpanded)
                    {
                        if (!string.IsNullOrEmpty(lastPropertyPath) && propertyIterator.propertyPath.Contains(lastPropertyPath))
                        {
                            continue;
                        }
                        EditorGUI.indentLevel++;
                        lastPropertyPath = propertyIterator.propertyPath;
                        EditorGUILayout.PropertyField(propertyIterator, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropertyPath) && propertyIterator.propertyPath.Contains(lastPropertyPath))
                    {
                        continue;
                    }
                    lastPropertyPath = propertyIterator.propertyPath;

                    EditorGUILayout.PropertyField(propertyIterator, drawChildren);
                    TryDrawScriptableObjectsEditor(propertyIterator);
                }
            }
            while (propertyIterator.NextVisible(false));
        }
    }

    protected void DrawFoldout(SerializedProperty propertyIterator, string prefix = "", string sufix = "")
    {
        EditorGUILayout.BeginHorizontal();
        propertyIterator.isExpanded = EditorGUILayout.Foldout(propertyIterator.isExpanded, prefix + propertyIterator.displayName + sufix);
        EditorGUILayout.EndHorizontal();
    }

    protected void TryDrawScriptableObjectsEditor(SerializedProperty propertyIterator)
    {
        // Let me edit scriptable objects without searching them
        object value = propertyIterator.boxedValue;
        if (value != null && value is ScriptableObject)
        {
            DrawFoldout(propertyIterator, "", " - Expand to inspect");
            if (propertyIterator.isExpanded)
            {
                EditorGUI.indentLevel++;
                Editor scriptableEditor = Editor.CreateEditor(value as ScriptableObject);
                scriptableEditor.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space(5);
        }
    }

    protected void DrawSidebar(SerializedProperty propertyIterator)
    {
        if (propertyIterator.NextVisible(true))
        {
            do
            {
                if (GUILayout.Button(propertyIterator.displayName))
                {
                    selectedPropertyPath = propertyIterator.propertyPath;
                }
            }
            while (propertyIterator.Next(false));

            if (!string.IsNullOrEmpty(selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            }
        }
    }

    protected void ApplyChanges()
    {
        if (serializedObject == null)
        {
            return;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
