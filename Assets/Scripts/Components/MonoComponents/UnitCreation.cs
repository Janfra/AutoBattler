using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitCreation : MonoBehaviour
    {
        [SerializeField]
        private SharedUnitSelection selectedUnit;
        private bool canInstantiateUnit;

        private void Awake()
        {
            canInstantiateUnit = false;
            selectedUnit.OnValueChanged += OnUnitSelectionUpdate;
        }

        public void TrySpawnSelectedUnitAt(Vector2 position)
        {
            if (!canInstantiateUnit)
            {
                return;
            }

            Debug.Log($"Spawning unit {selectedUnit.Value.UnitName} at {position}");
            selectedUnit.Value = null;
        }

        private void OnUnitSelectionUpdate()
        {
            if (selectedUnit.Value != null)
            {
                OnUnitSelectionReady();
            }
            else
            {
                OnNullUnitSelection();
            }
        }

        private void OnUnitSelectionReady()
        {
            canInstantiateUnit = true;
        }

        private void OnNullUnitSelection()
        {
            canInstantiateUnit = false;
        }
    }
}
