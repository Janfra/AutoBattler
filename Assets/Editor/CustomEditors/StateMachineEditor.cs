using UnityEditor;
using MyCustomEditor;
using GameAI;

[CustomEditor(typeof(ConditionStateMachine), true)]
public class ConditionStateMachineEditor : CustomGUIEditorWithWindow
{
    public override void OnOpenEditor()
    {
        CustomGUIEditorWindow.OpenWindow((ConditionStateMachine)target, "Conditional State Machine Editor");
    }
}

[CustomEditor(typeof(DesireStateMachine), true)]
public class DesireStateMachineEditor : CustomGUIEditorWithWindow
{
    public override void OnOpenEditor()
    {
        CustomGUIEditorWindow.OpenWindow((DesireStateMachine)target, "Desire State Machine Editor");
    }
}