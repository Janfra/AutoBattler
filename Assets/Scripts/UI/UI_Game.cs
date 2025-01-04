using UnityEngine;
using UnityEngine.UIElements;

namespace AutoBattler.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UI_Game : MonoBehaviour
    {
        public const string QUIT_BUTTON_NAME = "CloseButton";

        [SerializeField]
        private UIDocument uiGameObject;
        [SerializeField]
        private UI_UnitsSelection unitsSelectionUI = new UI_UnitsSelection();

        private Button quitButton;

        private void Awake()
        {
            if (uiGameObject == null)
            {
                uiGameObject = GetComponent<UIDocument>();
            }

            unitsSelectionUI.Initialise(uiGameObject);
            quitButton = uiGameObject.rootVisualElement.Q<Button>(QUIT_BUTTON_NAME);
            if (quitButton != null)
            {
                quitButton.clicked += Application.Quit;
            }
        }
    }
}
