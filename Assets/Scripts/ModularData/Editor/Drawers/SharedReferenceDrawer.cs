using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ModularData.Editor 
{ 
    [CustomPropertyDrawer(typeof(SharedReference<>), true)]
    public class SharedReferenceDrawer : PropertyDrawer
    {
        const string IsInternalValueName = "isInternal";
        const string InternalValueName = "internalValue";
        const string SharedValueName = "sharedValue";

        SerializedProperty rootProperty;
        VisualElement container;
        PropertyField valueField;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            rootProperty = property;

            SetHorizontalContainer();
            SerializedProperty isInternalProperty = rootProperty.FindPropertyRelative(IsInternalValueName);
            PropertyField isInternalField = SetupInternalField(isInternalProperty);
            SetupValueField(isInternalProperty.boolValue);

            isInternalField.label = property.displayName;
            container.Add(isInternalField);
            container.Add(valueField);
            root.Add(container);
            return root;
        }

        public void SetHorizontalContainer()
        {
            container = new VisualElement();

            container.style.flexDirection = FlexDirection.Row;
        }

        public PropertyField SetupInternalField(SerializedProperty isInternalProperty)
        {
            PropertyField isInternalField = new PropertyField();
            isInternalField.bindingPath = isInternalProperty.propertyPath;
            isInternalField.RegisterValueChangeCallback(OnInternalTypeChanged);
            isInternalField.tooltip = "Toggle sets whether to use a constant or a shared reference";

            return isInternalField;
        }

        public SerializedProperty SetupValueField(bool isInternal) 
        {
            valueField = new PropertyField();
            valueField.style.flexGrow = 1;
            return SetValueField(isInternal);
        }

        public SerializedProperty SetValueField(bool isInternal)
        {
            string propertyRelativeName = isInternal ? InternalValueName : SharedValueName;
            SerializedProperty selectedProperty = rootProperty.FindPropertyRelative(propertyRelativeName);
            valueField.bindingPath = selectedProperty.propertyPath;
            return selectedProperty;
        }

        public void OnInternalTypeChanged(SerializedPropertyChangeEvent eventCallback)
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
}
