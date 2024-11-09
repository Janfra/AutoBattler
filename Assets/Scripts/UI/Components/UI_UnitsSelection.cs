using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class UI_UnitsSelection
{
    [SerializeField]
    private VisualTreeAsset unitOptionVisualTreeAsset;

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
        AddUnitOption("Testo");
    }

    private void AddUnitOption(string unitName)
    {
        VisualElement root = unitOptionVisualTreeAsset.Instantiate();
        UI_UnitOption option = new UI_UnitOption(root, unitName);
        unitUIContainer.Add(option.UiRoot);
        unitOptions.Add(option);
    }
}

[Serializable]
public class UI_UnitOption
{
    const string UI_BUTTON_NAME = "Button";
    const string ROOT_NAME_PREFIX = "Unit Selection Button for ";

    public VisualElement UiRoot { get => uiRoot; }
    private VisualElement uiRoot;
    private Button selectionButton;
    private string unitName;

    public UI_UnitOption(VisualElement uiRoot, string unitName)
    {
        if (uiRoot == null)
        {
            throw new ArgumentNullException($"Trying to construct {GetType().Name} with a null UI Visual Element.");
        }

        this.uiRoot = uiRoot;
        this.unitName = unitName;

        uiRoot.name = ROOT_NAME_PREFIX + unitName;
        selectionButton = uiRoot.Q<Button>(UI_BUTTON_NAME);
        selectionButton.clicked += OnSelectUnit;
        selectionButton.text = unitName;
    }

    public void OnSelectUnit()
    {
        Debug.Log($"Selected unit {unitName}");
    }

    public void Unsubscribe()
    {
        Debug.Log($"Unsubscribed UI unit selection for {unitName}");
        selectionButton.clicked -= OnSelectUnit;
    }
}