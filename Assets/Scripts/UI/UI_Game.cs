using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UI_Game : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiGameObject;
    [SerializeField]
    private UI_UnitsSelection unitsSelectionUI = new UI_UnitsSelection();

    private void Awake()
    {
        if (uiGameObject == null)
        {
            uiGameObject = GetComponent<UIDocument>();
        }

        unitsSelectionUI.Initialise(uiGameObject);
    }
}
