using ModularData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoBattler.UI
{
    [Serializable]
    public class UI_UnitsSelection
    {
        [SerializeField]
        private VisualTreeAsset unitOptionVisualTreeAsset;
        [SerializeField]
        private SharedUnitSelection unitSelection;

        [SerializeField]
        private UnitDefinition test;

        private List<UI_UnitOption> unitOptions = new List<UI_UnitOption>();
        private VisualElement unitUIContainer;

        public void Initialise(UIDocument uiGameObject)
        {
            if (uiGameObject == null)
            {
                throw new ArgumentNullException($"{GetType().Name} component was given a null {typeof(UIDocument).Name} on {nameof(Initialise)}, unable to add Unit Options UI to screen.");
            }

            if (unitOptionVisualTreeAsset == null)
            {
                throw new NullReferenceException($"{uiGameObject.name} has a {GetType().Name} component with a null Visual Tree Asset on {nameof(Initialise)}, unable to create Unit Options UI");
            }

            unitUIContainer = uiGameObject.rootVisualElement.Q<VisualElement>("UnitGroupContainer");
            CreateUnitOption(test);
        }

        private void CreateUnitOption(UnitDefinition newUnitData)
        {
            VisualElement root = unitOptionVisualTreeAsset.Instantiate();
            UI_UnitOption option = new UI_UnitOption(root, newUnitData, SetSelectedUnit);
            unitUIContainer.Add(option.UiRoot);
            unitOptions.Add(option);
        }

        private void SetSelectedUnit(UnitDefinition selectedUnitData)
        {
            unitSelection.Value = selectedUnitData;
        }
    }

    [Serializable]
    public class UI_UnitOption
    {
        const string UI_BUTTON_NAME = "Button";
        const string ROOT_NAME_PREFIX = "Unit Selection Button for ";

        public delegate void OnSelected(UnitDefinition selectedUnitData);

        public VisualElement UiRoot { get => uiRoot; }
        private VisualElement uiRoot;
        private Button selectionButton;
        private UnitDefinition unitData;
        private OnSelected onSelected;

        public UI_UnitOption(VisualElement uiRoot, UnitDefinition unitData, OnSelected onSelected)
        {
            if (uiRoot == null)
            {
                throw new ArgumentNullException($"Trying to construct {GetType().Name} with a null UI Visual Element.");
            }

            this.uiRoot = uiRoot;
            this.unitData = unitData;
            this.onSelected = onSelected;

            uiRoot.name = ROOT_NAME_PREFIX + unitData.UnitName;
            selectionButton = uiRoot.Q<Button>(UI_BUTTON_NAME);
            selectionButton.clicked += OnSelectUnit;
            selectionButton.text = unitData.UnitName;
        }

        public void OnSelectUnit()
        {
            Debug.Log($"Selected unit {unitData.UnitName}");
            if (onSelected == null)
            {
                throw new NullReferenceException($"{GetType().Name} has been selected but the selection listener is null, unable to complete unit selection.");
            }

            onSelected.Invoke(unitData);
        }

        public void Unsubscribe()
        {
            Debug.Log($"Unsubscribed UI unit selection for {unitData.UnitName}");
            selectionButton.clicked -= OnSelectUnit;
        }
    }
}