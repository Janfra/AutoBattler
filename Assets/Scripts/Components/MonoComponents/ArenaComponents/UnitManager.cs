using ModularData;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField]
        private SharedUnitDefinitionSelection selectedUnit;
        
        [SerializeField]
        private ArenaBattleUnitsData teamAUnits;

        [SerializeField]
        private ArenaBattleUnitsData teamBUnits;

        [SerializeField]
        private ExtendedUnitDefinition extendedUnit;

        public bool HasSelectedUnit => selectedUnit.Value != null;
        private bool canInstantiateUnit;
        private Transform unitsContainer;

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

            unitsContainer = new GameObject("Unit Container").transform;
            unitsContainer.parent = transform;
        }

        private void OnApplicationQuit()
        {
            teamAUnits.CheckReset();
            teamBUnits.CheckReset();
        }

        public void SetSelectedUnit()
        {
            selectedUnit.Value = extendedUnit;
            canInstantiateUnit = true;
        }
        
        public void TrySpawnSelectedUnitAt(BattleTile spawningTile, PathfindRequester pathfindRequester, bool isTeamA = true)
        {
            if (!canInstantiateUnit)
            {
                return;
            }

            Debug.Log($"Spawning unit {selectedUnit.Value.UnitName} at {spawningTile.name}");
            ExtendedUnitDefinition unitDefinition = selectedUnit.Value as ExtendedUnitDefinition;
            BattleUnit unit = Instantiate(unitDefinition.UnitPrefab, spawningTile.transform);
            unit.transform.parent = unitsContainer;
            ExtendedBattleUnitData unitData = new ExtendedBattleUnitData(unit.transform, unit.Health, unitDefinition, spawningTile.pathfindHandler);

            if (isTeamA)
            {
                unit.Initialise(unitData, pathfindRequester, teamBUnits, teamAUnits);
                teamAUnits.AddValue(unitData);
            }
            else
            {
                unit.Initialise(unitData, pathfindRequester, teamAUnits, teamBUnits);
                teamBUnits.AddValue(unitData);
            }

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
