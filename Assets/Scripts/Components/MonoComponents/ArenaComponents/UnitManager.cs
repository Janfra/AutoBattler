using GameAI;
using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitManager : MonoBehaviour
    {
        protected struct SpawnData
        {
            public ExtendedUnitDefinition unitDefinition;
            public BattleTile spawnTile;
            public PathfindRequester pathfind;
            public ArenaBattleUnitsData friendlyUnits;
            public ArenaBattleUnitsData enemyUnits;

            public GraphNodeHandle GetPathfindNode()
            {
                return spawnTile.PathfindHandler;
            }
        }

        [SerializeField]
        private SharedUnitDefinitionSelection selectedUnit;
        
        [SerializeField]
        private ArenaBattleUnitsData teamAUnits;

        [SerializeField]
        private ArenaBattleUnitsData teamBUnits;

        [SerializeField]
        private ExtendedUnitDefinition enemyUnitTest;

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
            selectedUnit.Value = enemyUnitTest;
            canInstantiateUnit = true;
        }
        
        public void TrySpawnSelectedUnitAt(BattleTile spawningTile, PathfindRequester pathfindRequester, bool isTeamA = true)
        {
            if (!canInstantiateUnit)
            {
                return;
            }

            if (spawningTile == null || pathfindRequester == null)
            {
                throw new System.ArgumentNullException($"While trying to spawn an unit, {GetType().Name} was given null parameters in {nameof(TrySpawnSelectedUnitAt)} method - Object Name: {name}");
            }

            Debug.Log($"Spawning unit {selectedUnit.Value.UnitName} at {spawningTile.name}");
            SpawnData spawnData = new SpawnData();
            spawnData.unitDefinition = selectedUnit.Value as ExtendedUnitDefinition;
            spawnData.pathfind = pathfindRequester;
            spawnData.spawnTile = spawningTile;
            if (isTeamA)
            {
                spawnData.friendlyUnits = teamAUnits;
                spawnData.enemyUnits = teamBUnits;
            }
            else
            {
                spawnData.friendlyUnits = teamBUnits;
                spawnData.enemyUnits = teamAUnits;
            }

            SpawnUnit(spawnData);
            selectedUnit.Value = null;
        }

        private void SpawnUnit(SpawnData data)
        {
            BattleUnit unit = Instantiate(data.unitDefinition.UnitPrefab, data.spawnTile.transform);
            ExtendedBattleUnitData unitData = new ExtendedBattleUnitData(unit.transform, unit.Health, data.unitDefinition, data.GetPathfindNode());
            unit.Initialise(unitData, data.pathfind, data.enemyUnits, data.friendlyUnits);
            data.friendlyUnits.AddValue(unitData);
            unit.transform.parent = unitsContainer;
            unit.name = $"Unit {data.unitDefinition.name} #{data.friendlyUnits.Count} - {data.friendlyUnits.name}";
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
