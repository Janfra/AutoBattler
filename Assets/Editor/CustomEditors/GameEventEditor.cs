using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameEvent), true)]
public class GameEventEditor : Editor
{
    GameEvent gameEvent;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = base.CreateInspectorGUI();
        if (root == null)
        {
            root = new VisualElement();
        }

        gameEvent = (GameEvent)target;

        Button invokeButton = new Button();
        invokeButton.RegisterCallback<ClickEvent>(InvokeEvent);
        invokeButton.text = "Invoke Event";
        invokeButton.Bind(serializedObject);

        root.Add(invokeButton);
        return root;
    }

    public void InvokeEvent(ClickEvent callback)
    {
        if (gameEvent != null)
        {
            gameEvent.Invoke();
        }
    }
}
