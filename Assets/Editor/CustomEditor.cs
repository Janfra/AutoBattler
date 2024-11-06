using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyCustomEditor
{
    public abstract class CustomGUIEditorWithWindow : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                OnOpenEditor();
            }
        }

        public abstract void OnOpenEditor();
    }

    public abstract class CustomUIElementsEditor<T> : Editor 
        where T : UnityEngine.Object
    {
        public VisualTreeAsset VisualTree;
        protected VisualElement editorRoot;
        protected T editorTarget;

        public void OnEnable()
        {
            editorTarget = (T)target;
            OnEditorEnabled();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            if(VisualTree != null)
            {
                VisualTree.CloneTree(root);
                OnTreeCloned(root);
            }

            editorRoot = root;
            return root;
        }

        public virtual void OnTreeCloned(VisualElement root) { }
        public virtual void OnEditorEnabled() { }
    }
}