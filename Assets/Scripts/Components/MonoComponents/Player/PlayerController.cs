using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AutoBattler
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private BattleArena playerArena;

        private Camera cam;
        private AutoBattlerInput playerControls;
        private InputAction interact;
        private InputAction mousePosition;

        private void OnEnable()
        {
            interact = playerControls.Player.Interact;
            interact.Enable();
            interact.performed += OnInteract;

            mousePosition = playerControls.Player.Mouse;
            mousePosition.Enable();
        }

        private void OnDisable()
        {
            interact = playerControls.Player.Interact;
            interact.Disable();
            interact.performed -= OnInteract;

            mousePosition = playerControls.Player.Mouse;
            mousePosition.Disable();
        }

        private void Awake()
        {
            playerControls = new AutoBattlerInput();
            cam = Camera.main;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            playerArena.TrySpawnUnitAt(GetMouseWorldPosition());
        }

        public Vector2 GetMouseWorldPosition()
        {
            if (mousePosition == null || cam == null)
            {
                Debug.LogError($"Null value in {nameof(GetMouseWorldPosition)}");
                return Vector2.zero;
            }

            Vector3 mousePos = mousePosition.ReadValue<Vector2>();
            mousePos.z = Mathf.Abs(cam.transform.position.z);   
            Vector2 worldPoint = cam.ScreenToWorldPoint(mousePos);
            return worldPoint;
        }
    }
}
