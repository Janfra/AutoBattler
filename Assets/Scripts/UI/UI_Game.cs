using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Game : MonoBehaviour
{
    [SerializeField]
    private UIDocument gameUI;

    #region Unit Options UI Section
    [SerializeField]
    private VisualTreeAsset unitOptionUI;
    private List<UIUnitOption> unitOptions = new List<UIUnitOption>();
    private VisualElement unitUIContainer;

    [Serializable]
    protected struct UIUnitOption
    {
        public VisualElement uiRoot;
        public string unitName;
    }

    #endregion

    private void Start()
    {
        unitUIContainer = gameUI.rootVisualElement.Q<VisualElement>("UnitGroupContainer");
        AddUnitOption("Testo");
    }

    private void AddUnitOption(string unitName)
    {
        UIUnitOption option = new UIUnitOption();
        VisualElement root = unitOptionUI.Instantiate();
        root.name = $"Unit Selection Button: {unitName}";
        root.Q<Button>("Button").text = unitName;

        option.uiRoot = root;
        option.unitName = unitName;
        unitUIContainer.Add(option.uiRoot);
        unitOptions.Add(option);
    }
}
