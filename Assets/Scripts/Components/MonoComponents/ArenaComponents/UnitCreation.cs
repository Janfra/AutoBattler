using ModularData;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitCreation : MonoBehaviour
    {
        [SerializeField]
        private SharedUnitDefinitionSelection selectedUnit;
        
        [SerializeField]
        private ArenaBattleUnitsData teamAUnits;

        [SerializeField]
        private ArenaBattleUnitsData teamBUnits;

        public bool HasSelectedUnit => selectedUnit.Value != null;
        private bool canInstantiateUnit;

        private void Awake()
        {
            teamAUnits.CheckReset();
            teamBUnits.CheckReset();
            canInstantiateUnit = false;
            selectedUnit.OnValueChanged += OnUnitSelectionUpdate;
        }

        private void OnApplicationQuit()
        {
            teamAUnits.CheckReset();
            teamBUnits.CheckReset();
        }

        public void TrySpawnSelectedUnitAt(BattleTile spawningTile)
        {
            if (!canInstantiateUnit)
            {
                return;
            }

            Debug.Log($"Spawning unit {selectedUnit.Value.UnitName} at {spawningTile.name}");
            GameObject test = new GameObject("Test");
            test.transform.position = spawningTile.transform.position;
            BattleUnitData data = new BattleUnitData(test.transform, ScriptableObject.CreateInstance<UnitDefinition>());
            teamBUnits.AddValue(data);  

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
