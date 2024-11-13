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
            if (teamAUnits == null)
            {
                throw new System.NullReferenceException($"{nameof(teamAUnits)} reference is null in {GetType().Name} component. - Object Name: {name}");
            }
            teamAUnits.CheckReset();

            if (teamBUnits == null)
            {
                throw new System.NullReferenceException($"{nameof(teamBUnits)} reference is null in {GetType().Name} component. - Object Name: {name}");
            }
            teamBUnits.CheckReset();

            if (selectedUnit == null)
            {
                throw new System.NullReferenceException($"{nameof(selectedUnit)} reference is null in {GetType().Name} component. - Object Name: {name}");
            }

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
            HealthComponent testHp = test.AddComponent<HealthComponent>();
            testHp.SetMaxHealth(5);
            test.transform.position = spawningTile.transform.position;

            ExtendedBattleUnitData data = new ExtendedBattleUnitData(test.transform, testHp, ScriptableObject.CreateInstance<UnitDefinition>(), spawningTile.pathfindHandler);
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
